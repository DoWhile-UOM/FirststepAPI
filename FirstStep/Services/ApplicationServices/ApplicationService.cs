using System.ComponentModel.Design;
using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementService;

        public ApplicationService(DataContext context, IAdvertisementService advertisementService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _advertisementService = advertisementService;
        }

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

     
    
        public async Task Update(Application application)
        {
            Application dbApplication = await GetById(application.application_Id);

            dbApplication.email = application.email;
            dbApplication.status = application.status;
            dbApplication.phone_number = application.phone_number;
            dbApplication.submitted_date = application.submitted_date;

            await _context.SaveChangesAsync();           
        }

      

       

        // Nethma do these tasks

        // create a method to get all applications by advertisement id

        // create a methos to get all applications by seeker id

        // create a method to calculate all evaluated applications by advertisement id

        // create a method to calculate all un-evaluated applications by advertisement id

        // create a method to calculate all accepted applications by advertisement id

        // create a method to calculate all rejected applications by advertisement id

    }
}
