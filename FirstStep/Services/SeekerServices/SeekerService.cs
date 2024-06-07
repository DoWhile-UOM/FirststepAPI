using AutoMapper;
using FirstStep.Data;
using FirstStep.Helper;
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
                .Include("job_Field")
                .Include("skills")
                .ToListAsync();
        }

        public async Task<Seeker> GetById(int id)
        {
            Seeker? seeker = await _context.Seekers
                .Include("job_Field")
                .Include("skills")
                .Where(e => e.user_id == id)
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

        public async Task<SeekerApplicationDto> GetSeekerDetails(int id)
        {
            Seeker seeker = await GetById(id);
            SeekerApplicationDto seekerdto = _mapper.Map<SeekerApplicationDto>(seeker);
            return seekerdto;
        }

        public async Task<JobField> GetSeekerField(int seekerId)
        {
            Seeker seeker = await GetById(seekerId);

            if (seeker == null)
            {
                throw new NullReferenceException("Seeker not found.");
            }

            if (seeker.job_Field == null)
            {
                throw new NullReferenceException("Seeker's job field not found.");
            }

            return seeker.job_Field;
        }

        public async Task Create(AddSeekerDto newSeeker)
        {
            // map the AddSeekerDto to a Seeker object
            var seeker = _mapper.Map<Seeker>(newSeeker);

            // user type is seeker
            seeker.user_type = "seeker";

            if (newSeeker == null)
                throw new NullReferenceException("Null User");

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == seeker.email))
                throw new InvalidDataException("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(newSeeker.password);

            if (!string.IsNullOrEmpty(passCheck))
                throw new InvalidDataException(passCheck.ToString());

            //Hash password before saving to database
            seeker.password_hash = PasswordHasher.Hasher(newSeeker.password);

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
            dbSeeker.field_id = updateDto.field_id;


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

        public async Task<bool> IsValidSeeker(int seekerId)
        {
            var seeker = await _context.Seekers.Where(e => e.user_id == seekerId).FirstOrDefaultAsync();

            if (seeker == null)
            {
                return false;
            }

            return true;
        }

    }
}
