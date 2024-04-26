using FirstStep.Models.DTOs;
using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> GetById(int id);

        public Task<IEnumerable<HRManagerApplicationListDto>> GetHRManagerAdertisementListByJobID(int jobID);

        public Task<IEnumerable<Application>> GetBySeekerId(int id);

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