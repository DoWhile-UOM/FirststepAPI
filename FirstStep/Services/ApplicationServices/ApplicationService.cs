using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRevisionService _revisionService;
        private readonly IEmployeeService _employeeService;
        

        public ApplicationService(
            DataContext context, 
            IMapper mapper, 
            IRevisionService revisionService)
        {
            _context = context;
            _mapper = mapper;
            _revisionService = revisionService;
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

            Application newApplication = _mapper.Map<Application>(newApplicationDto);

            newApplication.status = ApplicationStatus.NotEvaluated.ToString();

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
                .Where(a => a.advertisement_id == id)
                .ToListAsync();
            
            if (applications is null)
            {
                throw new Exception("There are no applications under the advertisement");
            }

            return applications;
        }

        public async Task<IEnumerable<HRManagerApplicationListDto>> GetHRManagerAdertisementListByJobID(int jobID)
        {
            var applications = await FindByAdvertisementId(jobID);

            IEnumerable<HRManagerApplicationListDto> applicationList = new List<HRManagerApplicationListDto>();

            for (int i = 0; i < applications.Count(); i++)
            {
                HRManagerApplicationListDto application = _mapper.Map<HRManagerApplicationListDto>(applications.ElementAt(i));

                // find application status
                application.status = await _revisionService.GetCurrentStatus(application.application_Id);

                if (application.status != ApplicationStatus.NotEvaluated.ToString())
                {
                    application.is_evaluated = true;
                }

                applicationList.Append(application);
            }

            return applicationList;
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

        public async Task<int> NumberOfApplicationsByAdvertisementId(int id)
        {
            int NumberOfApplications = await _context.Applications.Where(a => a.advertisement_id == id).CountAsync();
            return NumberOfApplications;
        }

        public async Task<int> TotalEvaluatedApplications(int id)
        {
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.advertisement_id == id && a.status != ApplicationStatus.NotEvaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;
        }

        public async Task<int> TotalNotEvaluatedApplications(int id)
        {
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.advertisement_id == id && a.status == ApplicationStatus.NotEvaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;
        }

        public async Task<int> AcceptedApplications(int id)
        {
            int AcceptedApplications = await _context.Applications.Where(a => a.advertisement_id == id && a.status == ApplicationStatus.Accepted.ToString()).CountAsync();
            return AcceptedApplications;
        }

        public async Task<int> RejectedApplications(int id)
        {
            int AcceptedApplications = await _context.Applications.Where(a => a.advertisement_id == id && a.status == ApplicationStatus.Rejected.ToString()).CountAsync();
            return AcceptedApplications;
        }

        //Task delegation strats here

        //selecting applcations for evalution
        public async Task<IEnumerable<Application>> SelectApplicationsForEvaluation(int advertisement_id)
        {
            
            //get the advertisement
            var advertisement = await _context.Advertisements.FindAsync(advertisement_id);
            // Initialize applicationsOfTheAdvertisement as an empty list
            IEnumerable<Application> applicationsOfTheAdvertisement = new List<Application>();
            if (advertisement != null) {
                if (Enum.TryParse(advertisement.current_status, out AdvertisementValidation.Status status) &&
                status == AdvertisementValidation.Status.hold)
                {
                    if (AdvertisementValidation.IsExpired(advertisement))
                    {
                        applicationsOfTheAdvertisement = (await FindByAdvertisementId(advertisement.advertisement_id)).Where(a => a.assigned_hrAssistant_id == null);
                        return applicationsOfTheAdvertisement;
                    }

                }
                throw new NullReferenceException("No applications for evaluation."); // HTTP 204 No Content // should be replace with a suitable exception
            }
            throw new NullReferenceException("No applications for evaluation."); // HTTP 204 No Content
        }


        // initiating task delegation
        public async Task InitiateTaskDelegation(int company_id,int advertisement_id)
        {
            // Get all HR assistants for the specified company
            IEnumerable<Employee> hrAssistants = await _employeeService.GetAllHRAssistants(company_id);

            // Get applications that need evaluation for the specified company
            List<Application> applicationsForEvaluation = (await SelectApplicationsForEvaluation(advertisement_id)).ToList();

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

            // Delegate tasks to HR assistants
             await DelegateTask(hrAssistants.ToList(), applicationsForEvaluation);

            // Return a success response
             // HTTP 200 OK
        }


        // delagateTaks 
        public async Task DelegateTask(List<Employee> hrAssistants, List<Application> applications)
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
    }
}
