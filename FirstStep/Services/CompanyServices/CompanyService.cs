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

        public async Task<IEnumerable<Company>> GetAllUnregisteredCompanies()
        {
            return await _context.Companies.Where(c => !c.verification_status).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetAllRegisteredCompanies()
        {
            return await _context.Companies.Where(c => c.verification_status).ToListAsync();
        }

        public async Task Create(AddCompanyDto newCompanyDto)
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

        public async Task UpdateCompanyVerification(int companyID, CompanyRegInfoDto companyRegInfo)
        {
            var unRegCompany = await GetById(companyID);

            unRegCompany.verification_status = companyRegInfo.verification_status;
            unRegCompany.company_registered_date = companyRegInfo.company_registered_date;
            unRegCompany.verified_system_admin_id = companyRegInfo.verified_system_admin_id;
            unRegCompany.comment = companyRegInfo.comment;

            await _context.SaveChangesAsync();
        }

        public async Task RegisterCompany(int companyID, AddDetailsCompanyDto company)
        {
            var unRegCompany = await GetById(companyID);

            unRegCompany.company_logo = company.company_logo;
            unRegCompany.company_description = company.company_description;
            unRegCompany.company_city = company.company_city;
            unRegCompany.company_province = company.company_province;
            unRegCompany.company_business_scale = company.company_business_scale;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUnregisteredCompany(int companyID, UpdateUnRegCompanyDto company)
        {
            Company dbCompany = await GetById(companyID);

            dbCompany.company_name = company.company_name;
            dbCompany.company_email = company.company_email;
            dbCompany.business_reg_no = company.business_reg_no;
            dbCompany.company_website = company.company_website;
            dbCompany.company_phone_number = company.company_phone_number;
            dbCompany.verification_status = false;
            dbCompany.business_reg_certificate = company.business_reg_certificate;
            dbCompany.certificate_of_incorporation = company.certificate_of_incorporation;
            dbCompany.company_applied_date = company.company_applied_date;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRegisteredCompany(int companyID, UpdateCompanyDto company)
        {
            Company dbCompany = await GetById(companyID);

            dbCompany.company_name = company.company_name;
            dbCompany.company_email = company.company_email;
            dbCompany.business_reg_no = company.business_reg_no;
            dbCompany.company_website = company.company_website;
            dbCompany.company_phone_number = company.company_phone_number;
            dbCompany.company_logo = company.company_logo;
            dbCompany.company_description = company.company_description;
            dbCompany.company_city = company.company_city;
            dbCompany.company_province = company.company_province;
            dbCompany.company_business_scale = company.company_business_scale;

            await _context.SaveChangesAsync();
        }
    }
}
