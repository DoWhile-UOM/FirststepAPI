using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class RegisteredCompanyService //: IRegisteredCompanyService
    {
        /*
        private readonly DataContext _context;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public RegisteredCompanyService(DataContext context, ICompanyService companyService, IMapper mapper)
        {
            _context = context;
            _companyService = companyService;
            _mapper = mapper;
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

        public async Task SetAsRegistered(int companyID, RegisteredCompanyDto unregisteredCompany)
        {
            Company company = await _companyService.GetById(companyID);

            if (company == null)
            {
                throw new Exception("Company not found.");
            }

            // create new object which is a registered company
            RegisteredCompany newRegisteredCompany = new()
            {
                company_id = companyID,
                company_name = company.company_name,
                company_email = company.company_email,
                business_reg_no = company.business_reg_no,
                company_website = company.company_website,
                company_phone_number = company.company_phone_number,
                verification_status = true,
                business_reg_certificate = company.business_reg_certificate,
                certificate_of_incorporation = company.certificate_of_incorporation,
                company_logo = unregisteredCompany.company_logo,
                company_description = unregisteredCompany.company_description,
                company_city = unregisteredCompany.company_city,
                company_province = unregisteredCompany.company_province,
                company_business_scale = unregisteredCompany.company_business_scale,
                company_registered_date = unregisteredCompany.company_registered_date,
                verified_system_admin_id = unregisteredCompany.verified_system_admin_id
            };
            

            // remove form the company table
            _context.Companies.Remove(company);

            // set as registered
            _context.RegisteredCompanies.Add(newRegisteredCompany);

            await _context.SaveChangesAsync();
        }
        */
    }
}
