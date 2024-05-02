using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<AdvertisementFirstPageDto> GetAllWithPages(int seekerID, int pageLength);

        Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, int pageLength);

        Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, string city, int noOfResultsPerPage);

        Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID);

        Task<AdvertisementDto> GetById(int id);

        Task<UpdateAdvertisementDto> GetByIdWithKeywords(int id);

        Task<IEnumerable<Advertisement>> GetByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementTableRowDto>> GetByCompanyID(int companyID, string status);

        Task<IEnumerable<AdvertisementTableRowDto>> GetByCompanyID(int companyID, string status, string title);

        Task<IEnumerable<AdvertisementShortDto>> GetSavedAdvertisements(int seekerID);

        Task<IEnumerable<AppliedAdvertisementShortDto>> GetAppliedAdvertisements(int seekerID);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task SaveAdvertisement(int advertisementId, int seekerId, bool isSave);

        Task Delete(int id);

        Task DeleteWithApplications(int id);

        Task<AdvertisementFirstPageDto> CreateFirstPageResults(IEnumerable<Advertisement> dbAds, int seekerID, int noOfresultsPerPage);

        Task<AdvertisementFirstPageDto> BasicSearch(SearchJobRequestDto searchRequest, int seekerID, int pageLength);

        Task CloseExpiredAdvertisements();

        Task RemoveSavedExpiredAdvertisements();

        Task CreateApplication(AddApplicationDto newApplication);

        Task<bool> IsExpired(int jobID);
    }
}
