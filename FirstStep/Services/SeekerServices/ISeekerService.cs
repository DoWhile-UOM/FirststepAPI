using FirstStep.Models;
namespace FirstStep.Services
{
    public interface ISeekerService
    {
        public Task<IEnumerable<Seeker>> GetAll();

        public Task<SystemAdmin> GetById(int id);

        public Task Create(SystemAdmin systemAdmin);

        public Task Update(SystemAdmin systemAdmin);

        public Task Delete(int id);
    }
}
