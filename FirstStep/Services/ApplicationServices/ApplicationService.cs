using System.ComponentModel.Design;
using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;

        public ApplicationService(DataContext context)
        {
            _context = context;
        }

        enum AdvertisementStatus { Evaluated, NotEvaluated, Accepted, Rejected }

        public async Task Create(Application application) //task=>await _context
        {
            application.application_Id = 0;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Application application = await GetById(id);
            
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


        public async Task<IEnumerable<Application>> GetByAdvertisementId(int id)
        {
            ICollection<Application> applications = await _context.Applications.Where(a => a.advertisement_id == id).ToListAsync();
            if (applications is null)
            {
                throw new Exception("There are no applications under the advertisement");
            }
            return applications;
        }

        public async Task<IEnumerable<Application>> GetBySeekerId(int id)
        {
            ICollection<Application> applications = await _context.Applications.Where(a => a.user_id == id).ToListAsync();
            if (applications is null)
            {
                throw new Exception("There are no applications under the seeker");
            }
            return applications;
        }


        public async Task Update(Application application)
        {
            Application dbApplication = await GetById(application.application_Id);

            dbApplication.status = application.status;
            dbApplication.submitted_date = application.submitted_date;

            await _context.SaveChangesAsync();           
        }

        public async Task<int> TotalEvaluatedApplications(int id)
        {
            IEnumerable<Application> allapplications = await GetByAdvertisementId(id);
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.status == AdvertisementStatus.Evaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;

        }

        public async Task<int> TotalNotEvaluatedApplications(int id)
        {
            IEnumerable<Application> allapplications = await GetByAdvertisementId(id);
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.status == AdvertisementStatus.NotEvaluated.ToString()).CountAsync();
            return TolaEvaluatedApplications;

        }

        public async Task<int> AcceptedApplications(int id)
        {

            IEnumerable<Application> allapplications = await GetByAdvertisementId(id);
            int AcceptedApplications = await _context.Applications.Where(a => a.status == AdvertisementStatus.Accepted.ToString()).CountAsync();
            return AcceptedApplications;
        }

        public async Task<int> RejectedApplications(int id)
        {
            IEnumerable<Application> allapplications = await GetByAdvertisementId(id);
            int AcceptedApplications = await _context.Applications.Where(a => a.status == AdvertisementStatus.Rejected.ToString()).CountAsync();
            return AcceptedApplications;
        }



        // Nethma do these tasks

        // create a method to get all applications by advertisement id

        // create a methos to get all applications by seeker id

        // create a method to calculate all evaluated applications by advertisement id

        // create a method to calculate all un-evaluated applications by advertisement id

        // create a method to calculate all accepted applications by advertisement id

        // create a method to calculate all rejected applications by advertisement id

        // make a seeker Dto in including appliacation details

    }
}
