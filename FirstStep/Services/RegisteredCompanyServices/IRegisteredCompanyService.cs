using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IRegisteredCompanyService
    {
        public Task<IEnumerable<RegisteredCompany>> GetAll();

        public Task<RegisteredCompany> GetById(int id);

        public Task Update(RegisteredCompany registeredCompany);

        public Task Delete(int id);
    }
}
