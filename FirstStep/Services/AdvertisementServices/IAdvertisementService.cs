using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> FindAll();

        Task<Advertisement> FindById(int id);

        Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementShortDto>> GetAll();

        Task<AdvertisementDto> GetById(int id);

        Task<IEnumerable<JobOfferDto>> GetJobOffersByCompanyID(int companyID, string status);

        Task Create(AddAdvertisementDto advertisement);

        Task ChangeStatus(int id, string newStatus);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task Delete(int id);

        Task<IEnumerable<AdvertisementShortDto>> MapAdsToCardDtos(IEnumerable<Advertisement> advertisements);

        Task SearchAds();

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
