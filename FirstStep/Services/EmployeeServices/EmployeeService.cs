using AutoMapper;
using FirstStep.Data;
using FirstStep.Helper;
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
            Employee? employee = await _context.Employees
                .Include("company")
                .Where(e => e.user_id == id)
                .FirstOrDefaultAsync();

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
            ICollection<Employee> hrManagers = await _context.Employees
                .Where(e => e.company_id == company_Id && e.user_type == User.UserType.hrm.ToString())
                .ToListAsync();

            if (hrManagers is null)
            {
                throw new Exception("There are no HR Managers under the company");
            }

            return hrManagers;
        }

        public async Task<IEnumerable<Employee>> GetAllHRAssistants(int company_Id)
        {
            ICollection<HRAssistant> hrAssistants = await _context.HRAssistants
                .Where(e => e.company_id == company_Id && e.user_type == User.UserType.hra.ToString())
                .ToListAsync();
            if (hrAssistants is null)
            {
                throw new Exception("There are no HR Assistants under the company");
            }

            return hrAssistants;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(int company_Id)
        {
            ICollection<Employee> employees = await _context.Employees
                .Where(e => e.company_id == company_Id && e.user_type != User.UserType.ca.ToString())
                .ToListAsync();
            if (employees is null)
            {
                throw new Exception("There are no employees under the company");
            }

            return employees;
        }

        public async Task<IEnumerable<Employee>> GetEmployees(IEnumerable<int> emp_ids)
        {
            IEnumerable<Employee> employees = await _context.Employees.Where(e => emp_ids.Contains(e.user_id)).ToListAsync();
            return employees;
        }

        public async Task CreateHRManager(AddEmployeeDto newHRManager)
        {
            await ValidateCompany(newHRManager.company_id);
            
            var hrManager = _mapper.Map<HRManager>(newHRManager);

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == newHRManager.email))
                throw new Exception("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(newHRManager.password);

            if (!string.IsNullOrEmpty(passCheck))
                throw new Exception(passCheck);

            //Hash password before saving to database
            hrManager.password_hash = PasswordHasher.Hasher(newHRManager.password);

            hrManager.user_type = User.UserType.hrm.ToString();

            _context.HRManagers.Add(hrManager);
            await _context.SaveChangesAsync();
        }

        public async Task CreateHRAssistant(AddEmployeeDto newHRAssistant)
        {
            await ValidateCompany(newHRAssistant.company_id);

            var hrAssistant = _mapper.Map<HRAssistant>(newHRAssistant);

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == newHRAssistant.email))
                throw new Exception("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(newHRAssistant.password);

            if (!string.IsNullOrEmpty(passCheck))
                throw new Exception(passCheck);

            //Hash password before saving to database
            hrAssistant.password_hash = PasswordHasher.Hasher(newHRAssistant.password);
            hrAssistant.user_type = User.UserType.hra.ToString();

            _context.HRAssistants.Add(hrAssistant);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCompanyAdmin(AddCompanyAdminDto newCompanyAdmin)
        {
            // validate there is no any other company admin in within the company
            var company = await _context.Companies.Where(e => e.registration_url == newCompanyAdmin.company_registration_url).FirstOrDefaultAsync();

            if (company is null)
            {
                throw new Exception("Company not found");
            }
            
            if (company.company_admin_id != null)
            {
                throw new Exception("Company already has an admin");
            }

            var companyAdmin = _mapper.Map<HRManager>(newCompanyAdmin);

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == newCompanyAdmin.email))
                throw new Exception("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(newCompanyAdmin.password);

            if (!string.IsNullOrEmpty(passCheck))
                throw new Exception(passCheck);

            //Hash password before saving to database
            companyAdmin.password_hash = PasswordHasher.Hasher(newCompanyAdmin.password);

            companyAdmin.user_type = User.UserType.ca.ToString();
            companyAdmin.admin_company = company;
            companyAdmin.company_id = company.company_id;

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
            dbEmployee.user_type = employee.user_type;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Employee employee = await GetById(id);

            if (employee.user_type == User.UserType.ca.ToString())
            {
                throw new FieldAccessException("Company Admin cannot be deleted");
            }
            else if (employee.user_type == User.UserType.hrm.ToString())
            {
                // check whether the HR Manager has created any advertisement
                var advertisements = await _context.Advertisements.Where(a => a.hrManager_id == id).ToListAsync();

                if (advertisements.Count > 0)
                {
                    // find the company admin of hrm's company
                    int ca = (int)employee.company!.company_admin_id!;

                    // set the advertisements' created hr manager as the company admin of the deleting HR Manager's company
                    foreach (var ad in advertisements)
                    {
                        ad.hrManager_id = ca;
                    }
                }
            }

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
