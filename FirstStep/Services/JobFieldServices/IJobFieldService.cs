using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IJobFieldService
    {
        Task<IEnumerable<JobField>> GetAll();

        Task<JobField> GetById(int id);
        
        Task<JobField> Create(JobField jobField);

        Task<JobField> Update(JobField jobField);

        Task<JobField> Delete(int id);
    }
}
