using FirstStep.Data;
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class CompanyService : ICompanyService
    {
        public readonly DataContext _context;

        public CompanyService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetById(int id)
        {
            Company? company = await _context.Companies.FindAsync(id);
            if (company is null)
            {
                throw new Exception("JobField not found.");
            }

            return company;
        }

        public async Task Create(Company company)
        {
            company.company_id = 0;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Company reqCompany)
        {
            Company dbCompany = await GetById(reqCompany.company_id);

            dbCompany.company_name = reqCompany.company_name;
            dbCompany.business_reg_no = reqCompany.business_reg_no;
            dbCompany.company_email = reqCompany.company_email;
            dbCompany.company_website = reqCompany.company_website;
            dbCompany.company_phone_number = reqCompany.company_phone_number;
            dbCompany.verification_status = reqCompany.verification_status;
            dbCompany.business_reg_certificate = reqCompany.business_reg_certificate;
            dbCompany.certificate_of_incorporation = reqCompany.certificate_of_incorporation;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Company company = await GetById(id);

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
