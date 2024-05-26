using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Validation;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<HRManager> FindHRM(int id)
        {
            var hrmanager = await _context.HRManagers.FindAsync(id);
            if (hrmanager is null)
            {
                throw new Exception("HR Manager not found.");
            }

            return hrmanager;
        }

        public async Task<int> FindCompany(int id)
        {
            var employee = await GetById(id);
            return employee.company_id;
        }

        public async Task<IEnumerable<Employee>> GetAllHRManagers(int company_Id)
        {
            ICollection<Employee> hrManagers = await _context.Employees.Where(e => e.company_id == company_Id && e.user_type == "HRM").ToListAsync();
            if (hrManagers is null)
            {
                throw new Exception("There are no HR Managers under the company");
            }

            return hrManagers;
        }

        public async Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id)
        {
            ICollection<HRAssistant> hrAssistants = await _context.HRAssistants.Where(e => e.company_id == company_Id && e.user_type == "hra").ToListAsync();
            if (hrAssistants is null)
            {
                throw new Exception("There are no HR Assistants under the company");
            }

            return hrAssistants;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(int company_Id)
        {
            ICollection<Employee> employees = await _context.Employees.Where(e => e.company_id == company_Id && e.user_type != "CA").ToListAsync();
            if (employees is null)
            {
                throw new Exception("There are no employees under the company");
            }
            return employees;
        }

        public async Task CreateHRManager(AddEmployeeDto newHRManager)
        {
            await ValidateCompany(newHRManager.company_id);
            
            var hrManager = _mapper.Map<HRManager>(newHRManager);

            hrManager.password_hash = newHRManager.password;
            hrManager.user_type = "HRM";

            _context.HRManagers.Add(hrManager);
            await _context.SaveChangesAsync();
        }

        public async Task CreateHRAssistant(AddEmployeeDto newHRAssistant)
        {
            await ValidateCompany(newHRAssistant.company_id);

            var hrAssistant = _mapper.Map<HRAssistant>(newHRAssistant);

            hrAssistant.password_hash = newHRAssistant.password;
            hrAssistant.user_type = "HRA";

            _context.HRAssistants.Add(hrAssistant);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCompanyAdmin(AddEmployeeDto newCompanyAdmin)
        {
            await ValidateCompany(newCompanyAdmin.company_id);

            // validate there is no any other company admin in within the company
            var company = await _context.Companies.FindAsync(newCompanyAdmin.company_id);

            if (company is null)
            {
                throw new Exception("Company not found");
            }
            
            if (company.company_admin_id != null)
            {
                throw new Exception("Company already has an admin");
            }

            var companyAdmin = _mapper.Map<HRManager>(newCompanyAdmin);

            companyAdmin.password_hash = newCompanyAdmin.password;
            companyAdmin.user_type = "CA";
            companyAdmin.admin_company = company;

            _context.HRManagers.Add(companyAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int userId, UpdateEmployeeDto employee)
        {
            var dbEmployee = await GetById(userId);

            // need to use seperate dto (without password hash, be password) as UpdateEmployeeDto
            dbEmployee.first_name = employee.first_name;
            dbEmployee.last_name = employee.last_name;
            dbEmployee.email = employee.email;
            //dbEmployee.password_hash = employee.password_hash;
            dbEmployee.user_type = employee.user_type;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Employee employee = await GetById(id);

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        private async Task ValidateCompany(int companyID)
        {
            var company = await _context.Companies.FindAsync(companyID);

            if (company is null)
            {
                throw new Exception("Company not found");
            }

            if (!CompanyValidation.IsRegistered(company))
            {
                throw new Exception("Company is not verified");
            }
        }
    }
}
