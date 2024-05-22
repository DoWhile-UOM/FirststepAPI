using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Validation;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRevisionService _revisionService;

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

            var applications = await FindByAdvertisementId(jobID);

            List<ApplicationListDto> applicationList = new List<ApplicationListDto>();

            for (int i = 0; i < applications.Count(); i++)
            {
                Application dbApplication = applications.ElementAt(i);
                string applicationStatus = _revisionService.GetCurrentStatus(dbApplication);;

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

            applicationListPage.applicationList = applicationList;

            return applicationListPage;
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
    }
}
