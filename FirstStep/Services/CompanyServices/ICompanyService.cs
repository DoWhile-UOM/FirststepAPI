using FirstStep.Models;

namespace FirstStep.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<Company>> GetAll();

        public Task<Company> GetById(int id);

        public Task Create(Company company);

        public Task Update(Company company);

        public Task Delete(int id);
    }
}
