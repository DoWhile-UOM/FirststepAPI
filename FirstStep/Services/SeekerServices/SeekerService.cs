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
        private readonly IFileService _fileService;

        public SeekerService(DataContext context, IMapper mapper, ISkillService seekerSkillService, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _seekerSkillService = seekerSkillService;
            _fileService = fileService;
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

        public async Task<UpdateSeekerDto?> GetSeekerProfileById(int id)
        {
            Seeker seeker = await GetById(id);
            if (seeker == null)
            {
                return null;
            }

            var cvUrl = await _fileService.GetBlobUrl(seeker.CVurl);
            // Generate the full URL for the profile picture
            var profilePictureUrl = seeker.profile_picture != null? await _fileService.GetBlobUrl(seeker.profile_picture): null;

            var updateSeekerDto = new UpdateSeekerDto
            {
                email = seeker.email,
                first_name = seeker.first_name,
                last_name = seeker.last_name,
                phone_number = seeker.phone_number,
                bio = seeker.bio,
                description = seeker.description,
                university = seeker.university,
                CVurl = cvUrl, // Updated to fetch URL from file service
                profile_picture = profilePictureUrl, // Updated to fetch URL from file service
                linkedin = seeker.linkedin,
                field_id = seeker.field_id,
                seekerSkills = seeker.skills?.Select(s => s.skill_name).ToList()
            };

            return updateSeekerDto;
        }

        public async Task<SeekerApplicationDto> GetSeekerDetails(int id)
        {
            Seeker seeker = await GetById(id);
            SeekerApplicationDto seekerdto = _mapper.Map<SeekerApplicationDto>(seeker);
            //assign GetBlobUrl to seekerApplicationDto default_cv_url 
            seekerdto.defualt_cv_url = await _fileService.GetBlobUrl(seekerdto.cVurl);

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
            if (newSeeker == null)
                throw new NullReferenceException("Null User");

            //check if email already exists
            if (await _context.Users.AnyAsync(x => x.email == newSeeker.email))
                throw new InvalidDataException("Email Already exist");

            //password strength check
            var passCheck = UserCreateHelper.PasswordStrengthCheck(newSeeker.password);

            if (!string.IsNullOrEmpty(passCheck))
                throw new InvalidDataException(passCheck.ToString());
            
            // map the AddSeekerDto to a Seeker object
            var seeker = _mapper.Map<Seeker>(newSeeker);

            // user type is seeker
            seeker.user_type = "seeker";
            
            //Hash password before saving to database
            seeker.password_hash = PasswordHasher.Hasher(newSeeker.password);

            // Add skills to seeker
            seeker.skills = await IncludeSkillsToSeeker(newSeeker.seekerSkills);

            // Upload CV file and get the URL
            if (newSeeker.cvFile != null)
            {
                seeker.CVurl = await _fileService.UploadFile(newSeeker.cvFile);
            }

            // Upload profile picture file and get the URL
            if (newSeeker.profilePictureFile != null)
            {
                seeker.profile_picture = await _fileService.UploadFile(newSeeker.profilePictureFile);
            }

            _context.Seekers.Add(seeker);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int seekerId, UpdateSeekerDto updateDto)
        {
            Seeker dbSeeker = await GetById(seekerId);

            if (dbSeeker == null)
            {
                throw new KeyNotFoundException("Seeker not found.");
            }

            // Hash the new password if it has been changed and is not the placeholder
            if (!string.IsNullOrEmpty(updateDto.password) && updateDto.password != "********")
            {
                var passCheck = UserCreateHelper.PasswordStrengthCheck(updateDto.password);
                if (!string.IsNullOrEmpty(passCheck))
                {
                    throw new InvalidDataException(passCheck.ToString());
                }
                dbSeeker.password_hash = PasswordHasher.Hasher(updateDto.password);
            }
            dbSeeker.email = updateDto.email;
            dbSeeker.first_name = updateDto.first_name;
            dbSeeker.last_name = updateDto.last_name;
            dbSeeker.phone_number = updateDto.phone_number;
            dbSeeker.bio = updateDto.bio;
            dbSeeker.description = updateDto.description;
            dbSeeker.university = updateDto.university;
            dbSeeker.linkedin = updateDto.linkedin;
            dbSeeker.field_id = updateDto.field_id;

            dbSeeker.skills = await IncludeSkillsToSeeker(updateDto.seekerSkills);

            if (updateDto.cvFile != null)
            {
                dbSeeker.CVurl = await _fileService.UploadFile(updateDto.cvFile);
            }

            if (updateDto.profilePictureFile != null)
            {
                dbSeeker.profile_picture = await _fileService.UploadFile(updateDto.profilePictureFile);
            }

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

        public async Task<SeekerProfileViewDto> GetSeekerDetailsForSeekerProfileView(int id)
        {
            var seeker = await _context.Seekers
                .Include(s => s.skills)
                .FirstOrDefaultAsync(s => s.user_id == id);

            if (seeker == null)
            {
                throw new NullReferenceException("Seeker not found.");
            }

            // Generate the full URL for the CV and profile picture
            var cvUrl = await _fileService.GetBlobUrl(seeker.CVurl);
            var profilePictureUrl = seeker.profile_picture != null ? await _fileService.GetBlobUrl(seeker.profile_picture) : null;

            return new SeekerProfileViewDto
            {
                first_name = seeker.first_name,
                last_name = seeker.last_name,
                email = seeker.email,
                phone_number = seeker.phone_number,
                bio = seeker.bio,
                description = seeker.description,
                university = seeker.university,
                profile_picture = profilePictureUrl,
                linkedin = seeker.linkedin,
                field_id = seeker.field_id,
                user_id = seeker.user_id,
                cVurl = cvUrl,
                seekerSkills = seeker.skills?.Select(s => s.skill_name).ToList()
            };
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
