using System.Collections.Generic;
using System.Threading.Tasks;
using FirstStep.Models;

namespace FirstStep.Services.RegisteredCompanyServices

{
    public class RegisteredCompanyService : IRegisteredCompany
    {
        
        private readonly List<RegisteredCompany> _registeredCompanies = new List<RegisteredCompany>();

        public async Task<IEnumerable<RegisteredCompany>> GetAll()
        {
            return await Task.FromResult(_registeredCompanies);
        }

        public async Task<RegisteredCompany> GetById(int id)
        {
            return await Task.FromResult(_registeredCompanies.Find(company => company.company_id == id));
        }

        public async Task<Company> Create(RegisteredCompany registeredCompany)
        {
            _registeredCompanies.Add(registeredCompany);
            return await Task.FromResult(registeredCompany);
        }

        public void Update(RegisteredCompany registeredCompany)
        {
            var existingCompany = _registeredCompanies.Find(company => company.company_id == registeredCompany.company_id);
            if (existingCompany != null)
            {
                existingCompany.company_name = registeredCompany.company_name;
                existingCompany.company_email = registeredCompany.company_email;
            }
        }

        public void Delete(int id)
        {
            var companyToRemove = _registeredCompanies.Find(company => company.Id == id);
            if (companyToRemove != null)
            {
                _registeredCompanies.Remove(companyToRemove);
            }
        }
    }
}
