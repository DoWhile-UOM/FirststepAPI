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
        private readonly IAdvertisementService _advertisementService;

        public CompanyService(DataContext context, IMapper mapper, IAdvertisementService advertisementService)
        {
            _context = context;
            _mapper = mapper;
            _advertisementService = advertisementService;
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> FindByID(int id)
        {
            Company? company = await _context.Companies.FindAsync(id);
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            return company;
        }

        public async Task<CompanyProfileDetailsDto> GetById(int id)
        {
            Company company = await FindByID(id);
            CompanyProfileDetailsDto companydto = _mapper.Map<CompanyProfileDetailsDto>(company);
            return companydto;
        }

        public async Task<IEnumerable<Company>> GetAllUnregisteredCompanies()
        {
            return await _context.Companies.Where(c => !c.verification_status).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetAllRegisteredCompanies()
        {
            return await _context.Companies.Where(c => c.verification_status).ToListAsync();
        }
        
        public async Task<CompanyProfileDto> GetCompanyProfile(int companyID, int seekerID, int pageLength)
        {
            // get all active advertisements under the company
            IEnumerable<Advertisement> dbAdvertisements = await _advertisementService.GetByCompanyID(companyID);

            // get company details
            var dbCompany = await FindByID(companyID);

            // map to DTO
            var advertisementCompanyDto = _mapper.Map<CompanyProfileDto>(dbCompany);

            // feed all advertisments under the company to DTO as an array of advertisementCardDtos
            advertisementCompanyDto.companyAdvertisements = await _advertisementService.CreateFirstPageResults(dbAdvertisements, seekerID, pageLength);
            
            return advertisementCompanyDto;
        }

        //Company Registration Starts here
        public async Task Create(AddCompanyDto newCompanyDto)
        {
            var company = _mapper.Map<Company>(newCompanyDto);

            if (await CheckCompnayEmailExist(company.company_email))
            {
                throw new Exception("Company email already exists");
            }

            if (await CheckCompnayRegNo(company.business_reg_no.ToString()))
            {
                throw new Exception("Company registration number already exists");
            }

            company.verification_status = false;

            //Call Company Registration Email Verfication service

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> CheckCompnayEmailExist(string Email) //Function to check company email exist
        {
            return await _context.Companies.AnyAsync(x => x.company_email == Email);
        }

        private async Task<bool> CheckCompnayRegNo(string RegNo) //Function to check company regNo exist
        {
            return await _context.Companies.AnyAsync(x => x.business_reg_no == int.Parse(RegNo));
        }
        //Company Registration Ends here


        public async Task Delete(int id)
        {
            Company company = await FindByID(id);

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyVerification(int companyID, CompanyRegInfoDto companyRegInfo)
        {
            var unRegCompany = await FindByID(companyID);

            unRegCompany.verification_status = companyRegInfo.verification_status;
            unRegCompany.company_registered_date = companyRegInfo.company_registered_date;
            unRegCompany.verified_system_admin_id = companyRegInfo.verified_system_admin_id;
            unRegCompany.comment = companyRegInfo.comment;

            await _context.SaveChangesAsync();
        }

        public async Task RegisterCompany(int companyID, AddDetailsCompanyDto company)
        {
            var unRegCompany = await FindByID(companyID);

            unRegCompany.company_logo = company.company_logo;
            unRegCompany.company_description = company.company_description;
            unRegCompany.company_city = company.company_city;
            unRegCompany.company_province = company.company_province;
            unRegCompany.company_business_scale = company.company_business_scale;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUnregisteredCompany(int companyID, UpdateUnRegCompanyDto company)
        {
            Company dbCompany = await FindByID(companyID);

            if (dbCompany.verification_status)
            {
                throw new Exception("Company is already registered. Error API Call!");
            }

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
            Company dbCompany = await FindByID(companyID);

            if (!dbCompany.verification_status)
            {
                throw new Exception("Company is not registered. Error API Call!");
            }

            dbCompany.company_name = company.company_name;
            dbCompany.company_email = company.company_email;
            dbCompany.company_website = company.company_website;
            dbCompany.company_phone_number = company.company_phone_number;
            dbCompany.company_logo = company.company_logo;
            dbCompany.company_description = company.company_description;
            dbCompany.company_city = company.company_city;
            dbCompany.company_province = company.company_province;
            dbCompany.company_business_scale = company.company_business_scale;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRegistered(int companyID)
        {
            Company company = await FindByID(companyID);

            return company.verification_status;
        }
    }
}
