using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> FindAll();

        Task<Advertisement> FindById(int id);

        Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID);

        Task<IEnumerable<AdvertisementCardDto>> GetAll();

        Task<AdvertisementDto> GetById(int id);

        Task Create(AddAdvertisementDto advertisement);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task Delete(int id);

        IEnumerable<AdvertisementCardDto> MapAdsToCardDtos(IEnumerable<Advertisement> advertisements);

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
