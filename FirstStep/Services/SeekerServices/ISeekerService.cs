using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISeekerService
    {
        public Task<IEnumerable<Seeker>> GetAll();

        public Task<Seeker> GetById(int id);

        public Task<SeekerApplicationDto> GetSeekerDetails(int id);

        Task<SeekerProfileViewDto> GetSeekerDetailsForSeekerProfileView(int id); 

        public Task<JobField> GetSeekerField(int seekerId);

        //public Task Create(AddSeekerDto newSeeker);
      
        public Task<string> Create(AddSeekerDto newSeeker);

        public Task Update(int seekerID, Seeker seeker);

        public Task Delete(int id);

        public Task<bool> IsValidSeeker(int seekerId);
    }
}
