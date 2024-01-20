using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> GetAll();

        Task<Advertisement> GetById(int id);

        Task<Advertisement> Create(Advertisement advertisement);

        void Update(Advertisement advertisement);

        void Delete(int id);

        // IEnumerable<Advertisement> GetAdvertisementsByHRManagerId(int id);
    }
}
