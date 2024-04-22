using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> GetById(int id);

        public Task<IEnumerable<Application>> GetByAdvertisementId(int id);

        public Task<IEnumerable<Application>> GetBySeekerId(int id);

        public Task<SeekerApplicationDto> GetSeekr(int seekerID, int pageLength);

        public Task Create(Application application);

        public Task Update(Application application);

        public Task Delete(int id);

        public Task<int> NumberOfApplicationsByAdvertisementId(int id);

        public Task<int> TotalEvaluatedApplications(int id);

        public Task<int> TotalNotEvaluatedApplications(int id);

        public Task<int> AcceptedApplications(int id);

        public Task<int> RejectedApplications(int id);
    }
}