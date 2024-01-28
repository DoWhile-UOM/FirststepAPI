using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;
namespace FirstStep.Services
{
    public class SeekerService
    {
        private readonly DataContext _context;
        public SeekerService(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Seeker>> GetAll()
        {
            return await _context.Seekers.ToListAsync();
        }

        public async Task<Seeker> GetById(int id)
        {
            Seeker? seeker = await _context.Seekers.FindAsync(id);

            if (seeker is null)
            {
                throw new Exception("Seeker was not found");
            }

            return seeker;
        }

        public async Task Create(Seeker seeker)
        {
            seeker.user_id = 0;

            _context.Seekers.Add(seeker);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Seeker seeker)
        {
            Seeker dbSeeker = await GetById(seeker.user_id);

            dbSeeker.first_name = seeker.first_name;
            dbSeeker.last_name = seeker.last_name;
            dbSeeker.email = seeker.email;
            dbSeeker.phone_number = seeker.phone_number;
            dbSeeker.bio = seeker.bio;
            dbSeeker.description = seeker.description; 
            dbSeeker.university = seeker.university;
            dbSeeker.CVurl = seeker.CVurl;
            dbSeeker.profile_picture = seeker.profile_picture;
            dbSeeker.linkedin = seeker.linkedin;

        }

        public async Task Delete(int id)
        {
            Seeker seeker = await GetById(id);
            _context.Employees.Remove(seeker);
            await _context.SaveChangesAsync();
        }
    }
}
