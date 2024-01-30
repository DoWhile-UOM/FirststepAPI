using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class RegisteredCompanyService : ICompanyService, IRegisteredCompanyService
    {
        public RegisteredCompanyService(DataContext context) : base(context)
        {
        }

        public async Task<RegisteredCompany> Create(RegisteredCompany registeredCompany)
        {
            // Cast the RegisteredCompany to Company
            var company = (Company)registeredCompany;

            // Call the base class method to create a Company
            await base.Create(company);

            // Update the database with the RegisteredCompany specific properties
            await _context.Entry(company).GetDatabaseValuesAsync();
            registeredCompany.company_id = company.company_id;

            _context.RegisteredCompanies.Add(registeredCompany);
            await _context.SaveChangesAsync();

            return registeredCompany;
        }

        public async void Update(RegisteredCompany registeredCompany)
        {
            // Call the base class method to update the Company properties
            await base.Update(registeredCompany);

            // Update the database with the RegisteredCompany specific properties
            var dbRegisteredCompany = await _context.RegisteredCompanies
                .FirstOrDefaultAsync(c => c.company_id == registeredCompany.company_id);

            dbRegisteredCompany.company_logo = registeredCompany.company_logo;
            dbRegisteredCompany.company_description = registeredCompany.company_description;
            // Update other properties accordingly

            await _context.SaveChangesAsync();
        }
    }
}
