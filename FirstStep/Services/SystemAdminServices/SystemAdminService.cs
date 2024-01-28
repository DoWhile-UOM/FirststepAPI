using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class SystemAdminService : ISystemAdminService
    {
        private readonly DataContext _context;
        public SystemAdminService(DataContext context) {
            _context = context;
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

            _context.SystemAdmins.Add(systemAdmin);
            await _context.SaveChangesAsync();
        }
        public async Task Update(SystemAdmin systemAdmin)
        {
            SystemAdmin dbSystemAdmin= await GetById(systemAdmin.user_id);

            dbSystemAdmin.first_name = systemAdmin.first_name;
            dbSystemAdmin.last_name = systemAdmin.last_name;
            dbSystemAdmin.email = systemAdmin.email;
            dbSystemAdmin.password = systemAdmin.password;
        }

        public async Task Delete(int id)
        {
            SystemAdmin systemAdmin = await GetById(id);

            _context.SystemAdmins.Remove(systemAdmin);
            await _context.SaveChangesAsync();
        }

        

        
    }
}
