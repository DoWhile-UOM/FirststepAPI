using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class HRManagerService : IHRManagerService
    {
        private readonly DataContext _context;

        public HRManagerService(DataContext context)
        {
               _context = context;
        }

        public async Task<HRManager> Create(HRManager hrManager)
        {
            hrManager.user_id = 0;
            
            _context.HRManagers.Add(hrManager);
            await _context.SaveChangesAsync();

            return hrManager;
        }

        public async void Delete(int id)
        {
            HRManager hRManager = await GetId(id);

            _context.HRManagers.Remove(hRManager);
            await _context.SaveChangesAsync();
        }

        public async Task<HRManager> GetId(int id)
        {
            HRManager? hRManager = await _context.HRManagers.FindAsync(id);
            if(hRManager == null)
            {
                throw new Exception("HRManager not found");
            }
            return hRManager;
        }

        public async Task<IEnumerable<HRManager>> GetAll()
        {
            return await _context.HRManagers.ToListAsync();
        }

        public async void Update(HRManager hrManager)
        {
            //_context.Entry(hrManager).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            HRManager dbhRManager = await GetId(hrManager.user_id);
            dbhRManager.user_id = hrManager.user_id;
            dbhRManager.first_name = hrManager.first_name;
            dbhRManager.last_name = hrManager.last_name;
            dbhRManager.email = hrManager.email;
            dbhRManager.password = hrManager.password;
            await _context.SaveChangesAsync();
            //Does employee have this controller?


        }
    }
}
