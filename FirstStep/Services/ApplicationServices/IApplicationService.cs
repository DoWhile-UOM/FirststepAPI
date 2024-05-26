using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> GetById(int id);

        public Task<IEnumerable<HRManagerApplicationListDto>> GetHRManagerAdertisementListByJobID(int jobID);

        public Task<IEnumerable<Application>> GetBySeekerId(int id);

        public Task Create(AddApplicationDto newApplicationDto);

        public Task Update(Application application);

        public Task Delete(int id);

        public Task Delete(Application application);

        public string GetCurrentApplicationStatus(Application application);

        public Task<int> NumberOfApplicationsByAdvertisementId(int jobId);

        public Task<int> TotalEvaluatedApplications(int jobId);

        public Task<int> TotalNotEvaluatedApplications(int jobId);

        public Task<int> AcceptedApplications(int jobId);

        public Task<int> RejectedApplications(int jobId);

        //task delegation
        public Task<IEnumerable<Application>> SelectApplicationsForEvaluation(int advertisement_id);
        public Task InitiateTaskDelegation(int company_id, int advertisement_id);
        public Task DelegateTask(List<Employee> hrAssistants, List<Application> applications);
    }
}