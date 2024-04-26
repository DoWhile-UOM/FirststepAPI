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

        public async Task<int> GetSeekerField(int seekerId)
        {
            Seeker seeker = await GetById(seekerId);

            if (seeker == null)
            {
                throw new NullReferenceException("Seeker not found.");
            }

            return seeker.field_id;
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

        public async Task Update(int seekerId, Seeker seeker)
        {
            // For ruwandie, please follow these steps to update the Seeker object with relationships:

            // first create a DTO called UpdateSeekerDto
            // add all the properties that you want to update in the Seeker object
            // in the Seeker object, add a property called seekerSkills of type List<string>
            // in this function, need to map the Seeker object to the UpdateSeekerDto object
            // then update the Seeker object with the new values wihout skills

            // for to update skills, need to consider about relationship between Seeker and Skill (m to m)

            // for update the seeker's skills
            // first, find the already exists skill in the database
            // if the skill exists, add it to the seeker's list of skills
            // if the skill doesn't exist, create it and add it to the seeker's list of skills



            // this is the update function without updating the skills

            // for get the seeker object from the database
            Seeker dbSeeker = await GetById(seekerId);

            // update the Seeker object with the new values
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

            // update the Seeker's skills
            // Ruwanide you need to implement this part

            // save the changes
            await _context.SaveChangesAsync();


            // after implementing the above steps,
            // you should be change the function parameter type to UpdateSeekerDto
            // as well as the function return type to Task<UpdateSeekerDto>
            // also need to update the controller and the interface


            // you can refer the code segment in advertisement service for updating the relationships
            // line 180 to 212 - function name: IncludeProfessionKeywordsToAdvertisement()
            // advertisement and the keyword relationship is similar to the seeker and the skill relationship
        }

        public async Task Delete(int id)
        {
            Seeker seeker = await GetById(id);

            _context.Seekers.Remove(seeker);
            await _context.SaveChangesAsync();
        }
    }
}
