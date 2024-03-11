using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IApplicationService

    {

        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> GetById(int id);

      
        public Task Create(Application application);

        public Task Update(Application application);

        public Task Delete(int id);
    }
}