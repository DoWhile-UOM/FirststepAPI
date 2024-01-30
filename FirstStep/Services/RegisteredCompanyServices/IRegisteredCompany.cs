using FirstStep.Models;

namespace FirstStep.Services
{
    public interface IRegisteredCompany
    {
            public Task<IEnumerable<RegisteredCompany>> GetAll();

            public Task<RegisteredCompany> GetById(int id);

            public Task<Company> Create(RegisteredCompany registeredCompany);

            public void Update(RegisteredCompany registeredCompany);

            public void Delete(int id);
        
    }
}
