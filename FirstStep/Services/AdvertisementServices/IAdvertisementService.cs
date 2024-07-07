using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<AdvertisementFirstPageDto> GetAllWithPages(int seekerID, int pageLength);

        Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, int pageLength);

        Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, float longitude, float latitude, int noOfResultsPerPage);

        Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID);

        Task<AdvertisementDto> GetById(int id, int seekerID);

        Task<UpdateAdvertisementDto> GetByIdWithKeywords(int id);

        Task<IEnumerable<Advertisement>> GetByCompanyID(int companyID);
       
        Task<IEnumerable<AdvertismentTitleDto>> GetCompanyAdvertisementTitleList(int companyID);

        Task<IEnumerable<AdvertisementTableRowDto>> GetCompanyAdvertisementList(int emp_id, string status);

        Task<IEnumerable<AdvertisementTableRowDto>> GetCompanyAdvertisementList(int emp_id, string status, string title);

        Task<IEnumerable<AdvertisementHRATableRowDto>> GetAssignedAdvertisementsByHRA(int hra_userID);

        Task<IEnumerable<AdvertisementShortDto>> GetSavedAdvertisements(int seekerID);

        Task<IEnumerable<AppliedAdvertisementShortDto>> GetAppliedAdvertisements(int seekerID);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task ReactivateAdvertisement(int id, DateTime? submissionDeadline);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task SaveAdvertisement(int advertisementId, int seekerId, bool isSave);

        Task Delete(int id, bool isConfirmed);

        Task<AdvertisementFirstPageDto> CreateFirstPageResults(IEnumerable<Advertisement> dbAds, int seekerID, int noOfresultsPerPage);

        Task<AdvertisementFirstPageDto> BasicSearch(SearchJobRequestDto searchRequest, int seekerID, int pageLength);

        Task CloseExpiredAdvertisements();

        Task RemoveSavedExpiredAdvertisements();

        Task<bool> IsExpired(int jobID);
    }
}
