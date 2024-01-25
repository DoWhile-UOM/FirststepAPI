using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();

        public Task<Employee> GetById(int id);

        public Task<Employee> Create(HRManager employee);

        //void Update(int id, Employee item);
        public void Update(Employee employee);

        public void Delete(int id);
    }
}
