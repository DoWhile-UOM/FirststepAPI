using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRevisionService _revisionService;
        private readonly IFileService _fileService;
        private readonly IEmployeeService _employeeService;

        public ApplicationService(
            DataContext context,
            IMapper mapper,
            IRevisionService revisionService,
            IFileService fileService,
            IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _revisionService = revisionService;
            _fileService = fileService;
            _employeeService = employeeService;
        }

        public async Task SubmitApplication(AddApplicationDto newApplicationDto)
        {
            // get advertisement by id
            var advertisement = await _context.Advertisements.FindAsync(newApplicationDto.advertisement_id);

            // validate advertisement
            AdvertisementValidation.IsSutableForApply(advertisement);

            // get applications by seeker id
            var applications = await GetBySeekerId(newApplicationDto.seeker_id);

            foreach (var application in applications)
            {
                if (application.advertisement_id == newApplicationDto.advertisement_id
                    && application.seeker_id == newApplicationDto.seeker_id
                    && application.status == Application.ApplicationStatus.NotEvaluated.ToString())
                {
                    throw new InvalidDataException("Can't apply! another application is in the waiting list");
                }
            }

            await Create(newApplicationDto);
        }

        public async Task ResubmitApplication(AddApplicationDto newApplicationDto)
        {
            // get advertisement by id
            var advertisement = await _context.Advertisements.FindAsync(newApplicationDto.advertisement_id);

            // validate advertisement
            AdvertisementValidation.IsSutableForApply(advertisement);

            // get the seeker's default cv
            var seeker = await _context.Seekers.FindAsync(newApplicationDto.seeker_id);

            if (seeker == null)
            {
                throw new InvalidDataException("Seeker not found.");
            }

            // get application by seeker and advertisement
            var application = await _context.Applications
                .Include(a => a.revisions)
                .Where(a => a.advertisement_id == newApplicationDto.advertisement_id && a.seeker_id == newApplicationDto.seeker_id)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                await Create(newApplicationDto);
            }
            else
            {
                // update the application
                application.status = Application.ApplicationStatus.NotEvaluated.ToString();
                application.submitted_date = DateTime.Now;

                // remove the cv when it does not the seeker's defualt cv
                if (application.CVurl != seeker.CVurl)
                {
                    await _fileService.DeleteBlob(application.CVurl);
                }

                if (newApplicationDto.UseDefaultCv)
                {
                    // asign seeker's defualt cv
                    application.CVurl = seeker.CVurl;
                }
                else
                {
                    if (newApplicationDto.cv == null)
                    {
                        throw new InvalidDataException("cv file is required if not using the default cv");
                    }

                    // upload new cv
                    application.CVurl = await _fileService.UploadFile(newApplicationDto.cv);
                }

                // remove all revisions
                _context.Revisions.RemoveRange(application.revisions!);

                await _context.SaveChangesAsync();
            }
        }

        private async Task Create(AddApplicationDto newApplicationDto)
        {
            Application newApplication = _mapper.Map<Application>(newApplicationDto);
            newApplication.status = Application.ApplicationStatus.NotEvaluated.ToString();

            if (!newApplicationDto.UseDefaultCv)
            {
                if (newApplicationDto.cv == null)
                {
                    throw new InvalidDataException("cv file is required if not using the default cv");
                }

                // assign new cv
                newApplication.CVurl = await _fileService.UploadFile(newApplicationDto.cv);
            }
            else
            {
                //use default cv use in the seeker profile
                var seeker = await _context.Seekers.FindAsync(newApplicationDto.seeker_id);
                
                //handle the case where the seeker is not found
                if (seeker == null)
                {
                    throw new InvalidDataException("Seeker not found.");
                }
                
                newApplication.CVurl = seeker.CVurl;
            }

            _context.Applications.Add(newApplication);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(int id)
        {
            Application application = await FindById(id);

            // remove the cv from the blob storage when it does not match the seeker's default cv
            if (application.seeker!.CVurl != application.CVurl)
            {
                await _fileService.DeleteBlob(application.CVurl);
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Application application)
        {
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Application>> GetAll()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task<Application> GetById(int id)
        {
            return await FindById(id);
        }

        private async Task<Application> FindById(int id)
        {
            Application? application = await _context.Applications
                .Include(a => a.seeker)
                .Include(a => a.revisions)
                .Include(r => r.assigned_hrAssistant)
                .Where(a => a.application_Id == id)
                .FirstOrDefaultAsync();

            if (application is null)
            {
                throw new Exception("Application not found.");
            }

            return application;
        }

        private async Task<IEnumerable<Application>> FindByAdvertisementId(int id)
        {
            ICollection<Application> applications = await _context.Applications
                .Include("seeker")
                .Include("assigned_hrAssistant")
                .Include("revisions")
                .Where(a => a.advertisement_id == id)
                .ToListAsync();

            return applications;
        }

        public async Task<ApplicationListingPageDto> GetApplicationList(int jobID, string status)
        {
            var advertisement = await _context.Advertisements
                .Include("job_Field")
                .Include("hrManager")
                .FirstOrDefaultAsync(x => x.advertisement_id == jobID);

            if (advertisement is null)
            {
                throw new InvalidDataException("Advertisement not found.");
            }

            var applicationListPage = _mapper.Map<ApplicationListingPageDto>(advertisement);

            applicationListPage.applicationList = CreateApplicationList(await FindByAdvertisementId(jobID), status);

            return applicationListPage;
        }

        public async Task<ApplicationListingPageDto> GetAssignedApplicationList(int hraID, int jobID, string status)
        {
            var advertisement = await _context.Advertisements
                .Include("job_Field")
                .Include("applications")
                .Include("hrManager")
                .FirstOrDefaultAsync(x => x.advertisement_id == jobID);

            if (advertisement is null)
            {
                throw new InvalidDataException("Advertisement not found.");
            }

            var applicationListPage = _mapper.Map<ApplicationListingPageDto>(advertisement);

            if (advertisement.applications!.Count <= 0)
            {
                applicationListPage.applicationList = new List<ApplicationListDto>();
            }
            else
            {
                var applications = advertisement.applications!.Where(a => a.assigned_hrAssistant_id == hraID);

                // add seeker info to the application list
                applications = applications.Select(a =>
                {
                    a.seeker = _context.Seekers.Find(a.seeker_id);
                    return a;
                });

                applicationListPage.applicationList = CreateApplicationList(applications, status);
            }

            return applicationListPage;
        }

        private List<ApplicationListDto> CreateApplicationList(IEnumerable<Application> applications, string status)
        {
            List<ApplicationListDto> applicationList = new List<ApplicationListDto>();

            for (int i = 0; i < applications.Count(); i++)
            {
                Application dbApplication = applications.ElementAt(i);

                if (dbApplication.status != status && status != "all")
                {
                    continue;
                }

                var application = _mapper.Map<ApplicationListDto>(dbApplication);

                if (application.status != Application.ApplicationStatus.NotEvaluated.ToString())
                {
                    application.is_evaluated = true;
                }

                applicationList.Add(application);
            }

            return applicationList;
        }

        public async Task<ApplicationViewDto> GetSeekerApplications(int id)
        {
            var application = await FindById(id);

            if (application is null) 
            { 
                throw new NullReferenceException("Application not found."); 
            }

            // Get the latest revision
            var lastRevision = await _revisionService.GetLastRevision(application.application_Id);

            // Return the Application View DTO including the last revision details
            var applicationDto = _mapper.Map<ApplicationViewDto>(application.seeker);

            applicationDto.application_Id = application.application_Id;
            applicationDto.submitted_date = application.submitted_date;
            applicationDto.seeker_id = application.seeker_id;

            //applicationDto.cVurl = application.CVurl!; // when this is defualt cv, get from the seeker's profile
           
            // Fetch CV URL and profile picture URL from the file service
            var cvUrl = application.CVurl != null ? await _fileService.GetBlobUrl(application.CVurl) : await _fileService.GetBlobUrl(application.seeker!.CVurl);
            var profilePictureUrl = application.seeker!.profile_picture != null ? await _fileService.GetBlobUrl(application.seeker.profile_picture) : null;

            applicationDto.cVurl = cvUrl;
            applicationDto.profile_picture = profilePictureUrl;

            applicationDto.is_evaluated = lastRevision != null && lastRevision.status != Application.ApplicationStatus.NotEvaluated.ToString();
            applicationDto.current_status = application.status;

            if (lastRevision is not null)
            {
                applicationDto.last_revision = new RevisionDto
                {
                    revision_id = lastRevision.revision_id,
                    comment = lastRevision.comment!,
                    status = lastRevision.status,
                    created_date = lastRevision.date,
                    employee_id = lastRevision.employee_id,
                    name = lastRevision.employee!.first_name + " " + lastRevision.employee!.last_name,
                    role = lastRevision.employee!.user_type
                };
            }

            return applicationDto;
        }

        public async Task<IEnumerable<Application>> GetBySeekerId(int id)
        {
            // get all applications that send by the seeker and not completed
            var applications = await _context.Applications
                .Include("advertisement")
                .Where(a => a.seeker_id == id && a.status != Application.ApplicationStatus.Done.ToString()).ToListAsync();

            return applications;
        }

        public async Task Update(Application application)
        {
            Application dbApplication = await FindById(application.application_Id);

            dbApplication.status = application.status;
            dbApplication.submitted_date = application.submitted_date;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeAssignedHRA(int applicationId, int hrAssistantId)
        {
            // find the application
            Application application = await FindById(applicationId);

            // find the hr assistant
            if (await _employeeService.GetById(hrAssistantId) != null)
            {
                application.assigned_hrAssistant_id = hrAssistantId;

                await Update(application);
            }
        }

        //Task delegation strats here
        private async Task<List<Application>> SelectApplicationsForEvaluation(Advertisement advertisement)
        {
            // Initialize applicationsOfTheAdvertisement as an empty list
            List<Application> applicationsOfTheAdvertisement = new List<Application>();

            var stauts = advertisement.current_status;

            if (stauts == Advertisement.Status.hold.ToString() && AdvertisementValidation.IsExpired(advertisement))
            {
                applicationsOfTheAdvertisement = (await FindByAdvertisementId(advertisement.advertisement_id)).Where(a => a.assigned_hrAssistant_id == null).ToList();

                // return applications that need evaluate
                return applicationsOfTheAdvertisement;
            }

            throw new NullReferenceException("No applications for evaluation."); // HTTP 204 No Content
        }

        public async Task InitiateTaskDelegation(int advertisement_id, IEnumerable<int>? hrassistant_ids)
        {
            //get the advertisement
            var advertisement = await _context.Advertisements.Include("hrManager").Where(ad => ad.advertisement_id == advertisement_id).FirstOrDefaultAsync();

            if (advertisement == null)
            {
                throw new NullReferenceException("Advertisement not found."); // HTTP 204 No Content
            }

            if (hrassistant_ids is not null && hrassistant_ids.Count() >= 0)
            {
                var hrAssistants = await _employeeService.GetEmployees(hrassistant_ids);

                List<Application> applicationsForEvaluation = await GetApplicationsForTaskDelegation(advertisement, hrAssistants);

                // Delegate tasks to HR assistants
                await DelegateTask(hrAssistants.ToList(), applicationsForEvaluation);
            }
            else
            {
                await InitiateTaskDelegation(advertisement);
            }
        }

        public async Task InitiateTaskDelegation(Advertisement advertisement)
        {
            var hrAssistants = await _employeeService.GetAllHRAssistants(advertisement.hrManager!.company_id);

            List<Application> applicationsForEvaluation = await GetApplicationsForTaskDelegation(advertisement, hrAssistants);

            // Delegate tasks to HR assistants
            await DelegateTask(hrAssistants.ToList(), applicationsForEvaluation);
        }

        private async Task<List<Application>> GetApplicationsForTaskDelegation(Advertisement advertisement, IEnumerable<Employee> hrAssistants)
        {
            // Get applications that need evaluation for the specified company
            List<Application> applicationsForEvaluation = await SelectApplicationsForEvaluation(advertisement);

            // Check if there are no applications for evaluation
            if (!applicationsForEvaluation.Any())
            {
                throw new NullReferenceException("No applications for evaluation."); // HTTP 204 No Content
            }

            // Check if there are fewer than 2 HR assistants
            if (hrAssistants.Count() < 2)
            {
                throw new NullReferenceException("Not enough HR Assistants for task delegation."); // HTTP 400 Bad Request
            }

            return applicationsForEvaluation;
        }

        private async Task DelegateTask(List<Employee> hrAssistants, List<Application> applications)
        {
            var remainingApplications = applications.Count % hrAssistants.Count;
            var noOfApplicationsPerAssistant = (applications.Count - remainingApplications) / hrAssistants.Count;
            var noOfHrAssistants = hrAssistants.Count;

            for (int i = 0; i < noOfHrAssistants; i++)
            {
                for (int j = 0; j < noOfApplicationsPerAssistant; j++)
                {
                    applications[(i * noOfApplicationsPerAssistant + j)].assigned_hrAssistant_id = hrAssistants[i].user_id;
                    await Update(applications[(i * noOfApplicationsPerAssistant + j)]);
                }
            }

            for (int i = 0; i < remainingApplications; i++)
            {
                applications[(noOfHrAssistants * noOfApplicationsPerAssistant + i)].assigned_hrAssistant_id = hrAssistants[i].user_id;
                await Update(applications[(noOfHrAssistants * noOfApplicationsPerAssistant + i)]);
            }
        }
        //tasks delegation ends here

        public async Task<ApplicationStatusDto> GetApplicationStatus(int advertisementId, int seekerId)
        {
            var application = await _context.Applications
                .Include("advertisement")
                .Where(a => a.advertisement_id == advertisementId && a.seeker_id == seekerId)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                throw new NullReferenceException("Application not found.");
            }

            if (application.advertisement == null)
            {
                throw new NullReferenceException("Advertisement not found.");
            }

            var applicationStatus = new ApplicationStatusDto
            {
                //assign GetBlobUrl to dto cv name
                cv_name = await _fileService.GetBlobUrl(application.CVurl!),
                submitted_date = application.submitted_date,
                status = "",
                advertisement_id = advertisementId,
                seeker_id = seekerId
            };

            applicationStatus.status = GetApplicationStatus(application);

            return applicationStatus;
        }
        
        public async Task<IEnumerable<RevisionHistoryDto>> GetRevisionHistory(int applicationId)
        {
            var revisions = await _context.Revisions
                .Include(r => r.employee)
                .Where(r => r.application_id == applicationId)
                .OrderBy(r => r.date)  // Order revisions by date
                .ToListAsync();

            return revisions.Select(r => new RevisionHistoryDto
            {
                revision_id = r.revision_id,
                comment = r.comment,
                status = r.status,
                created_date = r.date,
                employee_name = r.employee!.first_name + " " + r.employee!.last_name,
                employee_role = r.employee!.user_type
            });
        }

        public string GetApplicationStatus(Application application)
        {
            if (application.advertisement is null)
            {
                throw new NullReferenceException("Advertisement not found.");
            }

            if (AdvertisementValidation.IsActive(application.advertisement))
            {
                return "Submitted";
            }
            else if (application.advertisement.current_status == Advertisement.Status.hold.ToString() &&
                (application.status == Application.ApplicationStatus.Pass.ToString() ||
                application.status == Application.ApplicationStatus.NotEvaluated.ToString()))
            {
                return "Screening";
            }
            else if (application.status == Application.ApplicationStatus.Accepted.ToString() ||
                (application.advertisement.current_status == Advertisement.Status.hold.ToString() &&
                application.status == Application.ApplicationStatus.Rejected.ToString()))
            {
                return "Finalized";
            }
            else
            {
                return "Rejected";
            }
        }


        //get selected application details as ApplicationSelectedDto 
        /*  public async Task<ApplicationSelectedDto> GetSelectedApplicationDetails(int applicationId)
          {
              //check the advertisment in hold state
              var application = await _context.Applications
                   .Include(a => a.seeker)
                   .Include(a => a.advertisement)
                   .Where(a => a.application_Id == applicationId)
                   .FirstOrDefaultAsync();

              if (application == null)
              {
                  throw new NullReferenceException("Application not found.");
              }

              if (application.advertisement == null)
              {
                  throw new NullReferenceException("Advertisement not found.");
              }

              if (application.advertisement.current_status != Advertisement.Status.hold.ToString())
              {
                  throw new InvalidDataException("Advertisement is not in hold state.");
              }
              //the application should be in accepted state
              if (application.status != Application.ApplicationStatus.Accepted.ToString())
              {
                  throw new InvalidDataException("Application is not in accepted state.");
              }
              var applicationSelectedDto = _mapper.Map<ApplicationSelectedDto>(application);
              applicationSelectedDto.seeker_name = application.seeker!.first_name + " " + application.seeker!.last_name;
              //get the last revision employee name 
              var lastRevision = await _revisionService.GetLastRevision(application.application_Id);
              if (lastRevision != null)
              {
                  applicationSelectedDto.last_revision_employee_name = lastRevision.employee!.first_name + " " + lastRevision.employee!.last_name;
              }
              return applicationSelectedDto;

          }*/

        //get selected applications details as ApplicationSelectedDto using advertisment ID
        public async Task<IEnumerable<ApplicationSelectedDto>> GetSelectedApplicationsDetails(int advertisementId)
        {
            //check the advertisment in hold state
            var advertisement = await _context.Advertisements
                .Include(a => a.applications)
                .Where(a => a.advertisement_id == advertisementId)
                .FirstOrDefaultAsync();

            if (advertisement == null)
            {
                throw new NullReferenceException("Advertisement not found.");
            }

            if (advertisement.current_status != Advertisement.Status.hold.ToString())
            {
                throw new InvalidDataException("Advertisement is not in hold state.");
            }

            //get the applications that are in accepted state
            var applications = advertisement.applications!.Where(a => a.status == Application.ApplicationStatus.Accepted.ToString());

            //get the selected applications details
            var applicationSelectedDtos = applications.Select(a =>
            {
                var applicationSelectedDto = _mapper.Map<ApplicationSelectedDto>(a);
                applicationSelectedDto.seeker_name = a.seeker!.first_name + " " + a.seeker!.last_name;
                //get the last revision employee name 
                var lastRevision = _revisionService.GetLastRevision(a.application_Id).Result;
                if (lastRevision != null)
                {
                    applicationSelectedDto.last_revision_employee_name = lastRevision.employee!.first_name + " " + lastRevision.employee!.last_name;
                }
                return applicationSelectedDto;
            });

            return applicationSelectedDtos;
        }



    }
}