using System.ComponentModel.Design;
using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ApplicationService(DataContext context)
        {
            _context = context;
        }

        enum ApplicationStatus { Evaluated, NotEvaluated, Accepted, Rejected }

        /* public async Task Create(Application application) 
         { 
         try
         {

             _context.Applications.Add(application);
             await _context.SaveChangesAsync();
         }
             catch (Exception e)
             {
             throw new Exception("Application not created", e);
         }
         }*/

        public async Task Create(AddApplicationDto application)
        {
            Application newApplication = new Application
            {
                advertisement_id = application.advertisement_id,
                user_id = application.user_id,
                submitted_date = application.submitted_date,
                status = application.status
            };

            _context.Applications.Add(newApplication);
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

        public async Task<int> NumberOfApplicationsByAdvertisementId(int id)
        {
            int NumberOfApplications = await _context.Applications.Where(a => a.advertisement_id == id).CountAsync();
            return NumberOfApplications;
        }

        public async Task<int> TotalEvaluatedApplications(int id)
        {
            int TolaEvaluatedApplications = await _context.Applications.Where(a => a.advertisement_id == id && a.status == ApplicationStatus.Evaluated.ToString()).CountAsync();
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

        public Task<SeekerApplicationDto> GetSeekerDetails(int seekerID)
        {
            throw new NotImplementedException();
        }

     
    }
}
