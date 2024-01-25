using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();

        public Task<Employee> GetById(int id);

        public Task<IEnumerable<Employee>> GetAllHRManagers(int company_Id);

        public Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id);

        public void CreateHRManager(HRManager hRManager);

        public void CreateHRAssistant(Employee hRAssistant);

        public void Update(Employee employee);

        public void Delete(int id);
    }
}
