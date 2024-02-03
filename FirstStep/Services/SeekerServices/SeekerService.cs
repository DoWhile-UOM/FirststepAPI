using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class SeekerService : ISeekerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ISkillService _seekerSkillService;

        public SeekerService(DataContext context, IMapper mapper, ISkillService seekerSkillService)
        {
            _context = context;
            _mapper = mapper;
            _seekerSkillService = seekerSkillService;
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

        public async Task Create(AddSeekerDto newSeeker)
        {
            var seeker = _mapper.Map<Seeker>(newSeeker);

            seeker.user_type = "seeker";

            

            _context.Seekers.Add(seeker);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int seekerId, Seeker seeker)
        {
            Seeker dbSeeker = await GetById(seekerId);

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

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Seeker seeker = await GetById(id);

            _context.Seekers.Remove(seeker);
            await _context.SaveChangesAsync();
        }
    }
}
