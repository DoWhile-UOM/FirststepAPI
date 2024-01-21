using FirstStep.Models;

namespace FirstStep.Services.JobFieldServices
{
    public interface IJobFieldService
    {
        Task<IEnumerable<JobField>> GetAll();

        Task<JobField> GetById(int id);
        
        Task<JobField> Create(JobField jobField);
        
        void Update(JobField jobField);
        
        void Delete(int id);
    }
}
