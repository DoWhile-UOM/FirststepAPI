using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementShortDto>> GetAll(int seekerID);

        Task<AdvertisementFirstPageDto> GetFirstPage(int seekerID, int pageLength);

        Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID);

        Task<AdvertisementDto> GetById(int id);

        Task<UpdateAdvertisementDto> GetByIdWithKeywords(int id);

        Task<IEnumerable<AdvertisementTableRowDto>> GetAdvertisementsByCompany(int companyID, string status);

        Task<IEnumerable<AdvertisementTableRowDto>> GetAdvertisementsByCompany(int companyID, string status, string title);

        Task<IEnumerable<AdvertisementShortDto>> GetSavedAdvertisements(int seekerID);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task SaveAdvertisement(int advertisementId, int seekerId, bool isSave);

        Task Delete(int id);

        Task<IEnumerable<AdvertisementShortDto>> CreateAdvertisementList(IEnumerable<Advertisement> advertisements, int seekerID);

        Task<AdvertisementFirstPageDto> BasicSearch(SearchJobRequestDto searchRequest, int seekerID, int pageLength);

        Task<IEnumerable<AdvertisementShortDto>> AdvanceSearch(SearchJobRequestDto requestAdsDto, int seekerID);
    }
}
