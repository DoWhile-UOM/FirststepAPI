using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CompanyService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(CompanyDto newCompanyDto)
        {
            var company = _mapper.Map<Company>(newCompanyDto);

            company.verification_status = false;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Company company = await GetById(id);

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
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
                throw new Exception("Company not found.");
            }

            return company;
        }

        public async Task Update(int companyID, Company company)
        {
            Company dbCompany = await GetById(companyID);

            dbCompany.company_name = company.company_name;
            dbCompany.company_email = company.company_email;
            dbCompany.business_reg_no = company.business_reg_no;
            dbCompany.company_website = company.company_website;
            dbCompany.company_phone_number = company.company_phone_number;
            dbCompany.verification_status = company.verification_status;
            dbCompany.business_reg_certificate = company.business_reg_certificate;
            dbCompany.certificate_of_incorporation = company.certificate_of_incorporation;

            await _context.SaveChangesAsync();
        }
    }
}
