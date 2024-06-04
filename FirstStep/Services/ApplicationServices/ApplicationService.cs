using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

        public enum ApplicationStatus { Pass, NotEvaluated, Accepted, Rejected, Done }

        public async Task Create(AddApplicationDto newApplicationDto)
        {
            // get advertisement by id
            var advertisement = await _context.Advertisements.FindAsync(newApplicationDto.advertisement_id);

            // validate advertisement
            if (advertisement is null)
            {
                throw new InvalidDataException("Advertisement not found.");
            }
            else if (AdvertisementValidation.IsExpired(advertisement))
            {
                throw new InvalidDataException("Advertisement is expired.");
            }
            else if (!AdvertisementValidation.IsActive(advertisement))
            {
                throw new InvalidDataException("Advertisement is not active.");
            }

            // get applications by seeker id
            var applications = await GetBySeekerId(newApplicationDto.seeker_id);

            foreach (var application in applications)
            {
                if (application.advertisement_id == newApplicationDto.advertisement_id
                    && application.seeker_id == newApplicationDto.seeker_id
                    && application.status == ApplicationStatus.NotEvaluated.ToString())
                {
                    throw new InvalidDataException("Can't apply for an advertisement that is already applied and in the waiting list");
                }
            }

            string cvBlobName = "";
            //use new cv
            if (!newApplicationDto.UseDefaultCv)
            {
                if (newApplicationDto.cv == null)
                {
                    throw new InvalidDataException("cv file is required if not using the default cv");
                }
                cvBlobName = await _fileService.UploadFileWithApplication(newApplicationDto.cv);
            }

            //upload cv file to Azure Blob Storage

            Application newApplication = _mapper.Map<Application>(newApplicationDto);

            newApplication.status = ApplicationStatus.NotEvaluated.ToString();

            //store cv file name in the database
            newApplication.CVurl = cvBlobName;

            _context.Applications.Add(newApplication);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Application application = await GetById(id);

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
            Application? application = await _context.Applications.FindAsync(id);

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
            var advertisement = await _context.Advertisements.Include("job_Field").FirstOrDefaultAsync(x => x.advertisement_id == jobID);

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
                string applicationStatus = _revisionService.GetCurrentStatus(dbApplication); ;

                if (applicationStatus != status && status != "all")
                {
                    continue;
                }

                var application = _mapper.Map<ApplicationListDto>(dbApplication);

                application.status = applicationStatus;

                if (application.status != ApplicationStatus.NotEvaluated.ToString())
                {
                    application.is_evaluated = true;
                }

                applicationList.Add(application);
            }

            return applicationList;
        }

        public async Task<ApplicationViewDto> GetSeekerApplicationViewByApplicationId(int id)
        {
            var application = await _context.Applications
                .Include("seeker")
                .Include("revisions")
                .SingleOrDefaultAsync(a => a.application_Id == id);

            if (application is null) { throw new NullReferenceException("Application not found."); }

            // Get the current application status
            string currentStatus = GetCurrentApplicationStatus(application);

            // Get the latest revision
            var lastRevision = application.revisions?.OrderByDescending(r => r.date).FirstOrDefault();

            return new ApplicationViewDto
            {
                application_Id = application.application_Id,
                submitted_date = application.submitted_date,
                email = application.seeker.email,
                first_name = application.seeker.first_name,
                last_name = application.seeker.last_name,
                phone_number = application.seeker.phone_number,
                bio = application.seeker.bio,
                cVurl = application.seeker.CVurl,
                profile_picture = application.seeker.profile_picture,
                current_status = currentStatus,  // Add the current status to the DTO
                last_revision = lastRevision == null ? null : new RevisionDto
                {
                    revision_id = lastRevision.revision_id,
                    comment = lastRevision.comment,
                    status = lastRevision.status,
                    created_date = lastRevision.date,
                    employee_id = lastRevision.employee_id
                }
            };

            //Application application = await GetById(id);

            //ApplicationViewDto applicationView = _mapper.Map<ApplicationViewDto>(application);

            //applicationView.revisionList = _revisionService.GetRevisionsByApplicationId(id);

            //return applicationView;
        }


        public async Task<IEnumerable<Application>> GetBySeekerId(int id)
        {
            // get all applications that send by the seeker and not completed
            var applications = await _context.Applications
                .Include("advertisement")
                .Where(a => a.seeker_id == id && a.status != ApplicationStatus.Done.ToString()).ToListAsync();

            return applications;
        }

        public async Task Update(Application application)
        {
            Application dbApplication = await GetById(application.application_Id);

            dbApplication.status = application.status;
            dbApplication.submitted_date = application.submitted_date;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeAssignedHRA(int applicationId, int hrAssistantId)
        {
            // find the application
            Application application = await GetById(applicationId);

            // find the hr assistant
            if (await _employeeService.GetById(hrAssistantId) != null)
            {
                application.assigned_hrAssistant_id = hrAssistantId;

                await Update(application);
            }
        }

        public string GetCurrentApplicationStatus(Application application)
        {
            if (application.revisions == null)
            {
                return ApplicationStatus.NotEvaluated.ToString();
            }

            // get last revision
            Revision lastRevision = application.revisions.OrderBy(a => a.date).Last();

            return lastRevision.status;
        }

        public async Task<int> NumberOfApplicationsByAdvertisementId(int jobId)
        {
            int NumberOfApplications = await _context.Applications.Where(a => a.advertisement_id == jobId).CountAsync();
            return NumberOfApplications;
        }

        public async Task<int> TotalEvaluatedApplications(int jobId)
        {
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.advertisement_id == jobId && a.status != ApplicationStatus.NotEvaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;
        }

        public async Task<int> TotalNotEvaluatedApplications(int jobId)
        {
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.advertisement_id == jobId && a.status == ApplicationStatus.NotEvaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;
        }

        public async Task<int> AcceptedApplications(int jobId)
        {
            int AcceptedApplications = await _context.Applications.Where(a => a.advertisement_id == jobId && a.status == ApplicationStatus.Accepted.ToString()).CountAsync();
            return AcceptedApplications;
        }

        public async Task<int> RejectedApplications(int jobId)
        {
            int AcceptedApplications = await _context.Applications.Where(a => a.advertisement_id == jobId && a.status == ApplicationStatus.Rejected.ToString()).CountAsync();
            return AcceptedApplications;
        }

        //Task delegation strats here
        private async Task<List<Application>> SelectApplicationsForEvaluation(Advertisement advertisement)
        {
            // Initialize applicationsOfTheAdvertisement as an empty list
            List<Application> applicationsOfTheAdvertisement = new List<Application>();

            var stauts = advertisement.current_status;

            if (stauts == AdvertisementValidation.Status.hold.ToString() && AdvertisementValidation.IsExpired(advertisement))
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

        //get application status by application id in seeker dashboard to look application status in stepper which shows submission,screening and finalize with dates
        public async Task<ApplicationStatusDto> GetApplicationStatus(int applicationId)
        {
            var application = await GetById(applicationId);
            if (application == null)
            {
                return null;
            }
            string currentStatus = GetCurrentApplicationStatus(application);
            var lastRevision = application.revisions?.OrderByDescending(r => r.date).FirstOrDefault();
            var applicationStatus = new ApplicationStatusDto
            {
                
                status = currentStatus,
                submitted_date = application.submitted_date,
                screening_date = lastRevision?.date ?? DateTime.Now,
                finalize_date = lastRevision?.date ?? DateTime.Now,
                cVurl = application.CVurl
            };

            return applicationStatus;
        }


    }

        //tasks delegation ends here
  }

