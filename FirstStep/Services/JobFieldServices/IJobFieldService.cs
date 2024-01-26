using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IJobFieldService
    {
        Task<IEnumerable<JobField>> GetAll();

        Task<JobField> GetById(int id);
        
        Task Create(JobField jobField);

        Task Update(JobField jobField);

        Task Delete(int id);
    }
}
