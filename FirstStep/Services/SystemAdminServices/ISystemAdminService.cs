using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISystemAdminService
    {
        public Task<IEnumerable<SystemAdmin>> GetAll();

        public Task<SystemAdmin> GetById(int id);

        public Task Create(SystemAdmin systemAdmin);

        public Task Update(SystemAdmin systemAdmin);

        public Task Delete(int id);
        public Task<LoggingsDto> GetLoggingsOfUsersAsync();
        public Task<IEnumerable<NotRegisteredEligibleCompanyDto>> GetEligibleUnregisteredCompanies();
    }
}
