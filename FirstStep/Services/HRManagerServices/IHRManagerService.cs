using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IHRManagerService
    {
        public Task<IEnumerable<HRManager>> GetAll();

        public Task<HRManager> GetId(int id);

        public Task<HRManager> Create(HRManager hrManager);

        //void Update(int id, HRManager item);
        public void Update(HRManager hrManager);

        public void Delete(int id);
    }
}
