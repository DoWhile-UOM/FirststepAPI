using FirstStep.Data;
using FirstStep.Helper;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace FirstStep.Services
{
    public class SystemAdminService : ISystemAdminService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public SystemAdminService(DataContext context, IUserService userService, ICompanyService companyService)
        {
            _context = context;
            _userService = userService;
            _companyService = companyService;
        }

        public async Task<IEnumerable<SystemAdmin>> GetAll()
        {
            return await _context.SystemAdmins.ToListAsync();
        }

        public async Task<SystemAdmin> GetById(int id)
        {
            SystemAdmin? systemAdmin = await _context.SystemAdmins.FindAsync(id);
            if (systemAdmin is null)
            {
                throw new Exception("SystemAdmin is not found.");
            }

            return systemAdmin;
        }

        public async Task Create(SystemAdmin systemAdmin)
        {
            systemAdmin.user_id = 0;
            systemAdmin.user_type = "sa";

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == systemAdmin.email))
                throw new Exception("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(systemAdmin.password_hash);

            if (!string.IsNullOrEmpty(passCheck))
                throw new Exception(passCheck);

            //Hash password before saving to database
            systemAdmin.password_hash = PasswordHasher.Hasher(systemAdmin.password_hash);

            _context.SystemAdmins.Add(systemAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task Update(SystemAdmin systemAdmin)
        {
            SystemAdmin dbSystemAdmin = await GetById(systemAdmin.user_id);

            dbSystemAdmin.first_name = systemAdmin.first_name;
            dbSystemAdmin.last_name = systemAdmin.last_name;
            dbSystemAdmin.email = systemAdmin.email;
            dbSystemAdmin.password_hash = systemAdmin.password_hash;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            SystemAdmin systemAdmin = await GetById(id);

            _context.SystemAdmins.Remove(systemAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task<LoggingsDto> GetLoggingsOfUsersAsync()
        {
            List<ActiveUserDto> activeUsers = await _userService.GetActiveUsersAsync();
            List<ActiveUserDto> inactiveUsers = await _userService.GetInactiveUsersAsync();

            int tot_active = activeUsers.Count() - (activeUsers.Count(user => user.user_type == "sa"));
            int tot_inactive = inactiveUsers.Count() - (inactiveUsers.Count(user => user.user_type == "sa"));

            int tot_cmpny_active_users = tot_active - activeUsers.Count(user => user.user_type == "seeker");
            int tot_cmpny_inactive_users = tot_inactive - inactiveUsers.Count(user => user.user_type == "seeker");

            int eligible_unregistered_companies_count = await _companyService.GetEligibleUnregisteredCompaniesCount();
            var loggingsDto = new LoggingsDto
            {
                activeTot = tot_active,
                inactiveTot = tot_inactive,
                activeCA = activeUsers.Count(user => user.user_type == "ca"),
                inactiveCA = inactiveUsers.Count(user => user.user_type == "ca"),
                activeHRM = activeUsers.Count(user => user.user_type == "hrm"),
                inactiveHRM = inactiveUsers.Count(user => user.user_type == "hrm"),
                activeHRA = activeUsers.Count(user => user.user_type == "hra"),
                inactiveHRA = inactiveUsers.Count(user => user.user_type == "hra"),
                activeSeeker = activeUsers.Count(user => user.user_type == "seeker"),
                inactiveSeeker = inactiveUsers.Count(user => user.user_type == "seeker"),
                activeCmpUsers = tot_cmpny_active_users,
                inactiveCmpUsers = tot_cmpny_inactive_users,
                eligibleUnregisteredCompaniesCount= eligible_unregistered_companies_count,
            };
            return loggingsDto;

        }

        public async Task<IEnumerable<NotRegisteredEligibleCompanyDto>> GetEligibleUnregisteredCompanies()
        {
            return await _companyService.GetEligibleUnregisteredCompanies();
        }

    }
}
