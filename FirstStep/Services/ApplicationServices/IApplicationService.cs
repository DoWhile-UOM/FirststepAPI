using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAll();

        Task<Application> GetById(int id);

        Task<Application> Create(Application application);

        void Update(Application application);

        void Delete(int id);
    }
}