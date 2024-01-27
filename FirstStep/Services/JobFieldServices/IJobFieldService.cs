using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IJobFieldService
    {
        public Task<IEnumerable<JobField>> GetAll();

        public Task<JobField> GetById(int id);
        
        public Task Create(JobField jobField);

        public Task Update(JobField jobField);

        public Task Delete(int id);
    }
}
