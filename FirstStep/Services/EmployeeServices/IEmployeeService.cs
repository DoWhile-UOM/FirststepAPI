using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();

        public Task<Employee> GetById(int id);

        public Task<IEnumerable<Employee>> GetAllHRManagers(int company_Id);

        public Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id);

        public Task<IEnumerable<Employee>> GetAllEmployees(int company_Id);

        public Task CreateHRManager(HRManager hRManager);

        public Task CreateHRAssistant(HRAssistant hRAssistant);

        public Task CreateCompanyAdmin(CompanyAdmin companyAdmin);

        public Task Update(Employee employee);

        public Task Delete(int id);
    }
}
