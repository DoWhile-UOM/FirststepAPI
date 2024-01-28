using FirstStep.Models;

namespace FirstStep.Services
{
    public interface ISeekerService
    {
        public Task<IEnumerable<Seeker>> GetAll();

        public Task<Seeker> GetById(int id);

        public Task Create(Seeker seeker);

        public Task Update(Seeker seeker);

        public Task Delete(int id);
    }
}
