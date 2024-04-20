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

        public async Task Create(AddSeekerDto newSeeker)
        {
            // map the AddSeekerDto to a Seeker object
            var seeker = _mapper.Map<Seeker>(newSeeker);

            // user type is seeker
            seeker.user_type = "seeker";

            // Add skills to seeker
            if (newSeeker.seekerSkills != null)
            {
                seeker.skills = new List<Skill>();

                foreach (var skill in newSeeker.seekerSkills)
                {
                    // check whether the skill exists in the database
                    var dbSkill = await _seekerSkillService.GetByName(skill);

                    if (dbSkill != null)
                    {
                        // if it exists, add it to the seeker's list of skills
                        seeker.skills.Add(dbSkill);
                    }
                    else
                    {
                        // if it doesn't exist, create it and add it to the seeker's list of skills
                        seeker.skills.Add(new Skill
                        {
                            skill_id = 0,
                            skill_name = skill
                        });
                    }
                }
            }

            _context.Seekers.Add(seeker);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int seekerId, UpdateSeekerDto updateDto)
        {
            
            // for get the seeker object from the database
            Seeker dbSeeker = await GetById(seekerId);

            // update the Seeker object with the new values
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

            // update the Seeker's skills
            dbSeeker.skills = await IncludeSkillsToSeeker(updateDto.seekerSkills);


            // save the changes
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
                        // if it exists, add it to the seeker's list of skills
                        skills.Add(dbSkill);
                    }
                    else
                    {
                        // if it doesn't exist, create a new skill and add it to the seeker's list of skills
                        var newSkill = new Skill
                        {
                            skill_id = 0,
                            skill_name = skillName.ToLower()  // Assuming the Skill class has a SkillName property
                        };
                   
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
