using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();

        public Task<Employee> GetById(int id);

        public Task<Employee> GetAllHRManagers(int company_Id);

        public Task<Employee> GetAllHRAssistants(int company_Id);

        public Task<Employee> Create(HRManager employee);

        //public Task<Employee> CreateHRManager(HRManager hRManager);

        public void Update(Employee employee);

        public void Delete(int id);
    }
}
