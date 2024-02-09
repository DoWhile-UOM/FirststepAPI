using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> GetAll();

        Task<Advertisement> FindById(int id);

        Task<AdvertisementDto> GetById(int id);

        Task Create(AddAdvertisementDto advertisement);

        Task Update(int jobID, UpdateAdvertisementDto advertisement);

        Task Delete(int id);

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
