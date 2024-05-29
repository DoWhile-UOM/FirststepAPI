using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IApplicationService
    {
        public Task<IEnumerable<Application>> GetAll();

        public Task<Application> GetById(int id);

        public Task<ApplicationListingPageDto> GetApplicationList(int jobID, string status);

        public Task<IEnumerable<Application>> GetBySeekerId(int id);

        public Task Create(AddApplicationDto newApplicationDto);

        public Task Update(Application application);

        public Task ChangeAssignedHRA(int applicationId, int hrAssistantId);

        public Task Delete(int id);

        public Task Delete(Application application);

        public string GetCurrentApplicationStatus(Application application);

        public Task InitiateTaskDelegation(int advertisement_id, IEnumerable<int>? hrassistant_ids);
    }
}