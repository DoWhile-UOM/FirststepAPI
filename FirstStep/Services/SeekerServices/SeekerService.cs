using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Grpc.Core;
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
            return await _context.Seekers
                .Include(e => e.job_Field)
                .Include(e => e.skills)
                .ToListAsync();
        }

        public async Task<Seeker> GetById(int id)
        {
            Seeker? seeker = await _context.Seekers
                .Where(e => e.user_id == id)
                .Include(e => e.job_Field)
                .Include(e => e.skills)
                .FirstOrDefaultAsync();

            if (seeker is null)
            {
                throw new Exception("Seeker was not found");
            }

            return seeker;
        }

        public async Task<UpdateSeekerDto> GetSeekerProfile(int id)
        {
            Seeker seeker = await GetById(id);

            return _mapper.Map<UpdateSeekerDto>(seeker);
        }

        public async Task Create(AddSeekerDto newSeeker)
        {
            var seeker = _mapper.Map<Seeker>(newSeeker);

            seeker.user_type = "seeker";

            seeker.skills = await IncludeSkillsToSeeker(newSeeker.seekerSkills);

            _context.Seekers.Add(seeker);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int seekerId, UpdateSeekerDto updateDto)
        {   
            Seeker dbSeeker = await GetById(seekerId);

            dbSeeker.first_name = updateDto.first_name;
            dbSeeker.last_name = updateDto.last_name;
            dbSeeker.email = updateDto.email;
            dbSeeker.phone_number = updateDto.phone_number;
            dbSeeker.bio = updateDto.bio;
            dbSeeker.description = updateDto.description;
            dbSeeker.university = updateDto.university;
            dbSeeker.CVurl = updateDto.CVurl;
            dbSeeker.profile_picture = updateDto.profile_picture;
            dbSeeker.linkedin = updateDto.linkedin;

            dbSeeker.skills = await IncludeSkillsToSeeker(updateDto.seekerSkills);

            await _context.SaveChangesAsync();
        }

        private async Task<ICollection<Skill>?> IncludeSkillsToSeeker(ICollection<string>? newSkills)
        {
            var skills = new List<Skill>();

            if (newSkills != null)
            {
                foreach (var skillName in newSkills)
                {
                    var dbSkill = await _seekerSkillService.GetByName(skillName.ToLower());

                    if (dbSkill != null)
                    {
                        skills.Add(dbSkill);
                    }
                    else
                    {
                        skills.Add(new Skill
                        {
                            skill_id = 0,
                            skill_name = skillName.ToLower()  
                        });
                   
                    }
                }
                return skills;
            }
            return null;
        }

        public async Task Delete(int id)
        {
            Seeker seeker = await GetById(id);

            _context.Seekers.Remove(seeker);
            await _context.SaveChangesAsync();
        }
    }
}
