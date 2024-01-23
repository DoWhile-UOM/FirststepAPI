using FirstStep.Models;

namespace FirstStep.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<Company>> GetAll();

        public Task<Company> GetById(int id);

        public Task<Company> Create(Company company);

        public void Update(Company company);

        public void Delete(int id);
    }
}
