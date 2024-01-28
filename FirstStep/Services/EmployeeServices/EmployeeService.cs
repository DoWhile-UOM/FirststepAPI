using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;

        public EmployeeService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            Employee? employee = await _context.Employees.FindAsync(id);
            if (employee is null)
            {
                throw new Exception("Employee not found.");
            }

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllHRManagers(int company_Id)
        {
            ICollection<Employee> hrManagers = await _context.Employees.Where(e => e.is_HRM && e.company_id == company_Id).ToListAsync();
            if (hrManagers is null)
            {
                throw new Exception("There are no HR Managers under the company");
            }

            return hrManagers;
        }

        public async Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id)
        {
            ICollection<Employee> hrAssistants = await _context.Employees.Where(e => !e.is_HRM && e.company_id == company_Id).ToListAsync();
            if (hrAssistants is null)
            {
                throw new Exception("There are no HR Assistants under the company");
            }

            return hrAssistants;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(int company_Id)
        {
            ICollection<Employee> employees = await _context.Employees.Where(e => e.company_id == company_Id).ToListAsync();
            if (employees is null)
            {
                throw new Exception("There are no employees under the company");
            }

            return employees;
        }

        public async Task CreateHRManager(HRManager hRManager)
        {
            hRManager.user_id = 0;
            hRManager.is_HRM = true;

            // check whether the company is a registered company using registeredCompany Service class

            _context.HRManagers.Add(hRManager);
            await _context.SaveChangesAsync();
        }

        public async Task CreateHRAssistant(HRAssistant hRAssistant)
        {
            hRAssistant.user_id = 0;
            hRAssistant.is_HRM = false;

            // check whether the company is a registered company using registeredCompany Service class

            _context.HRAssistants.Add(hRAssistant);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCompanyAdmin(CompanyAdmin companyAdmin)
        {
            companyAdmin.user_id = 0;
            companyAdmin.is_HRM = true;

            // check whether the company is a registered company using registeredCompany Service class
            // company id is not needed for company admin

            _context.CompanyAdmins.Add(companyAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Employee employee)
        {
            Employee dbEmployee = await GetById(employee.user_id);

            dbEmployee.first_name = employee.first_name;
            dbEmployee.last_name = employee.last_name;
            dbEmployee.email = employee.email;
            dbEmployee.password = employee.password;
            dbEmployee.is_HRM = employee.is_HRM;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Employee employee = await GetById(id);

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
