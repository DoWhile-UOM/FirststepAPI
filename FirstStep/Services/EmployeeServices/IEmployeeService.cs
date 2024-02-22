using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();

        public Task<Employee> GetById(int id);

        public Task<HRManager> FindHRM(int id);

        public Task<IEnumerable<Employee>> GetAllHRManagers(int company_Id);

        public Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id);

        public Task<IEnumerable<Employee>> GetAllEmployees(int company_Id);

        public Task CreateHRManager(AddEmployeeDto newHRManager);

        public Task CreateHRAssistant(AddEmployeeDto newHRAssistant);

        public Task CreateCompanyAdmin(AddEmployeeDto newCompanyAdmin);

        public Task Update(int id, UpdateEmployeeDto employee);

        public Task Delete(int id);
    }
}
