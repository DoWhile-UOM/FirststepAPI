using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class RegisteredCompanyService : IRegisteredCompanyService
    {
        private readonly DataContext _context;

        public RegisteredCompanyService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegisteredCompany>> GetAll()
        {
            return await _context.RegisteredCompanies.ToListAsync();
        }

        public async Task<RegisteredCompany> GetById(int id)
        {
            RegisteredCompany? registeredCompany = await _context.RegisteredCompanies.FindAsync(id);
            if (registeredCompany is null)
            {
                throw new Exception("Company not found.");
            }

            return registeredCompany;
        }

        public async Task Update(RegisteredCompany registeredCompany)
        {
            RegisteredCompany dbRegisteredCompany = await GetById(registeredCompany.company_id);

            dbRegisteredCompany.company_name = registeredCompany.company_name;
            dbRegisteredCompany.company_email = registeredCompany.company_email;
            dbRegisteredCompany.business_reg_no = registeredCompany.business_reg_no;
            dbRegisteredCompany.company_website = registeredCompany.company_website;
            dbRegisteredCompany.company_phone_number = registeredCompany.company_phone_number;
            dbRegisteredCompany.verification_status = registeredCompany.verification_status;
            dbRegisteredCompany.business_reg_certificate = registeredCompany.business_reg_certificate;
            dbRegisteredCompany.certificate_of_incorporation = registeredCompany.certificate_of_incorporation;
            dbRegisteredCompany.company_logo = registeredCompany.company_logo;
            dbRegisteredCompany.company_description = registeredCompany.company_description;
            dbRegisteredCompany.company_city = registeredCompany.company_city;
            dbRegisteredCompany.company_province = registeredCompany.company_province;
            dbRegisteredCompany.company_business_scale = registeredCompany.company_business_scale;
            dbRegisteredCompany.company_registered_date = registeredCompany.company_registered_date;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            RegisteredCompany registeredCompany = await GetById(id);

            _context.RegisteredCompanies.Remove(registeredCompany);
            await _context.SaveChangesAsync();
        }
    }
}
