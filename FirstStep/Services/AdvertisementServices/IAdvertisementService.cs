using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> FindAll();

        Task<Advertisement> FindById(int id);

        Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementShortDto>> GetAll(int seekerID);

        Task<AdvertisementDto> GetById(int id);

        Task<IEnumerable<JobOfferDto>> GetJobOffersByCompanyID(int companyID, string status);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task SaveAdvertisement(int advertisementId, int seekerId);

        Task UnsaveAdvertisement(int advertisementId, int seekerId);

        Task Delete(int id);

        Task<IEnumerable<AdvertisementShortDto>> MapAdsToCardDtos(IEnumerable<Advertisement> advertisements, int seekerID);

        Task SearchAds();

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
