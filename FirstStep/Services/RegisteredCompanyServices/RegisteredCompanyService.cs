using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FirstStep.Services.RegisteredCompanyServices
{
    public class RegisteredCompanyService : IRegisteredCompanyService
    {
        private readonly DataContext _context;

        public RegisteredCompanyService(DataContext context) {
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
                throw new Exception("Registered Company not found.");
            }

            return registeredCompany;
        }

        public async Task Update(RegisteredCompany reqRegisteredCompany)
        {
            RegisteredCompany dbRegisteredCompany = await GetById(reqRegisteredCompany.company_id);
            
            dbRegisteredCompany.company_name = reqRegisteredCompany.company_name;
            dbRegisteredCompany.business_reg_no = reqRegisteredCompany.business_reg_no;
            dbRegisteredCompany.company_email = reqRegisteredCompany.company_email;
            dbRegisteredCompany.company_website = reqRegisteredCompany.company_website;
            dbRegisteredCompany.company_phone_number = reqRegisteredCompany.company_phone_number;
            dbRegisteredCompany.verification_status = reqRegisteredCompany.verification_status;
            dbRegisteredCompany.business_reg_certificate= reqRegisteredCompany.business_reg_certificate;
            dbRegisteredCompany.certificate_of_incorporation=reqRegisteredCompany.certificate_of_incorporation;
            dbRegisteredCompany.company_logo=reqRegisteredCompany.company_logo;
            dbRegisteredCompany.company_description=reqRegisteredCompany.company_description;
            dbRegisteredCompany.company_city=reqRegisteredCompany.company_city;
            dbRegisteredCompany.company_province=reqRegisteredCompany.company_province;
            dbRegisteredCompany.company_business_scale=reqRegisteredCompany.company_business_scale;
            dbRegisteredCompany.company_registered_date = reqRegisteredCompany.company_registered_date;
            
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
