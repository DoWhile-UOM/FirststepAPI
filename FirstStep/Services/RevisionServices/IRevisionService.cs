using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IRevisionService
    {
        public Task<IEnumerable<Revision>> GetAll();

        public Task<Revision> GetById(int id);

        public Task<List<Revision>> GetByApplicationID(int applicationID)

        public Task<string> GetCurrentStatus(int applicationID);

        public Task Create(Revision revision);

        public Task Update(Revision revision);

        public Task Delete(int id);
    }
}
