using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> GetAll();

        Task<Advertisement> GetById(int id);

        void Create(AddAdvertisementDto advertisement);

        void Update(Advertisement advertisement);

        void Delete(int id);

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
