using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementShortDto>> GetAll(int seekerID);

        Task<AdvertisementDto> GetById(int id);

        Task<AddAdvertisementDto> GetByIdWithKeywords(int id);

        Task<IEnumerable<JobOfferDto>> GetAdvertisementsByCompany(int companyID, string status);

        Task<IEnumerable<JobOfferDto>> GetAdvertisementsByCompany(int companyID, string status, string title);

        Task<IEnumerable<AdvertisementShortDto>> GetSavedAdvertisements(int seekerID);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task SaveAdvertisement(int advertisementId, int seekerId, bool isSave);

        Task Delete(int id);

        Task<IEnumerable<AdvertisementShortDto>> CreateAdvertisementList(IEnumerable<Advertisement> advertisements, int seekerID);

        Task<IEnumerable<AdvertisementShortDto>> BasicSearch(SearchJobRequestDto searchRequest, int seekerID);

        Task<IEnumerable<AdvertisementShortDto>> AdvanceSearch(SearchJobRequestDto requestAdsDto, int seekerID);

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
