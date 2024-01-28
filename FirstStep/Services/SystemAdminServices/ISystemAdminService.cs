using FirstStep.Models;

namespace FirstStep.Services.SystemAdminServices
{
    public interface ISystemAdminService
    {
        Task<IEnumerable<SystemAdmin>> GetAll();

        Task<SystemAdmin> GetById(int id);

        Task Create(SystemAdmin systemAdmin);

        Task Update(SystemAdmin systemAdmin);

        Task Delete(int id);
    }
}
