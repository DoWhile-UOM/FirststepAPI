using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> FindById(int id);

        public Task<IEnumerable<Application>> FindByJobId(int advertisementID);

        public Task<IEnumerable<Application>> GetBySeekerId(int id);

        public Task Create(Application application);

        public Task Update(Application application);

        public Task Delete(int id);

        public Task<int> TotalEvaluatedApplications(int advertisementId);

        public Task<int> TotalNotEvaluatedApplications(int advertisementId);

        public Task<int> AcceptedApplications(int advertisementId);

        public Task<int> RejectedApplications(int advertisementId);
    }
}