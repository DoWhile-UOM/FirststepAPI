using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<Company>> GetAll();

        public Task<Company> GetById(int id);

        public Task Create(CompanyDto company);

        public Task Update(int companyID, Company company);

        public Task Delete(int id);
    }
}
