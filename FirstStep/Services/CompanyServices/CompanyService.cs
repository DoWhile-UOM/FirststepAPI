using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private readonly DataContext _context;

        public CompanyService(DataContext context)
        {
            _context = context;
        }

        public async Task<Company> Create(Company company)
        {
            company.company_id = 0;

            _context.Companys.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async void Delete(int id)
        {
            Company company = await GetById(id);

            _context.Companys.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _context.Companys.ToListAsync();
        }

        public async Task<Company> GetById(int id)
        {
            Company? company = await _context.Companys.FindAsync(id);
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            return company;
        }

        public async void Update(Company company)
        {
            Company dbCompany = await GetById(company.company_id);

            dbCompany.company_name = company.company_name;
            dbCompany.company_email = company.company_email;
            dbCompany.business_reg_no = company.business_reg_no;
            dbCompany.company_website = company.company_website;
            dbCompany.company_phone_number = company.company_phone_number;
            dbCompany.verification_status = company.verification_status;
            dbCompany.business_reg_certificate = company.business_reg_certificate;
            dbCompany.certificate_of_incorporation = company.certificate_of_incorporation;
        }
    }
}
