using FirstStep.Data;
using FirstStep.Models;

namespace FirstStep.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;

        public ApplicationService(DataContext context)
        {
            _context = context;
        }

        public async Task<Application> Create(Application application) //task=>await _context
        {
            application.application_Id = 0;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return application;
        }

        public  async void Delete(int id)
        {
            Application application = await GetById(id);
            
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Application>> GetAll()
        {
            throw new NotImplementedException();
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

        public async void Update(Application application)
        {
            Application dbApplication = await GetById(application.application_Id);

            //dbApplication.
        }
    }
}
