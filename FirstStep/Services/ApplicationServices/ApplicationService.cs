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

            
            dbApplication.status = application.status;
          
            dbApplication.submitted_date = application.submitted_date;

            await _context.SaveChangesAsync();           
        }
    }
}
