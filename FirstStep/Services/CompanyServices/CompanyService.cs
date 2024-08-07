﻿using AutoMapper;
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
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        // Random ID generation
        private static readonly Random random = new Random();
        private static readonly HashSet<string> seenIds = new HashSet<string>();

        public CompanyService(IEmailService emailService, DataContext context, IMapper mapper, IAdvertisementService advertisementService, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _advertisementService = advertisementService;
            _emailService = emailService;
            _fileService = fileService;
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
            if (companydto.company_logo != null)
            {
                companydto.company_logo = await _fileService.GetBlobUrl(companydto.company_logo);
            }
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

        public async Task<IEnumerable<ViewCompanyListDto>> GetAllCompanyList()
        {
            IEnumerable<Company> companies = await GetAll();
            IEnumerable<ViewCompanyListDto> companyDtos = _mapper.Map<IEnumerable<ViewCompanyListDto>>(companies);
            return companyDtos;
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
            
            if (dbCompany.company_logo == null)
            {
                advertisementCompanyDto.company_logo = "";
            }
            else
            {
                advertisementCompanyDto.company_logo = await _fileService.GetBlobUrl(dbCompany.company_logo);
            }

            return advertisementCompanyDto;
        }


        public async Task<CompanyApplicationDto> GetCompanyApplicationById(int companyID)
        {
            Company company = await FindByID(companyID);
            CompanyApplicationDto companydto = _mapper.Map<CompanyApplicationDto>(company);
            if(companydto.business_reg_certificate != null)
            {
                companydto.business_reg_certificate = await _fileService.GetBlobUrl(companydto.business_reg_certificate);
            }
            if (companydto.certificate_of_incorporation != null) 
            {
                companydto.certificate_of_incorporation = await _fileService.GetBlobUrl(companydto.certificate_of_incorporation);
            }
            if (company.company_logo != null)
            {
                companydto.company_logo = await _fileService.GetBlobUrl(company.company_logo);
            }
            
            return companydto;
        }
        
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
            if (newCompanyDto.company_logo != null)
            {
                company.company_logo = await _fileService.UploadFile(newCompanyDto.company_logo);
            }
            if (newCompanyDto.certificate_of_incorporation != null)
            {
                company.certificate_of_incorporation = await _fileService.UploadFile(newCompanyDto.certificate_of_incorporation);
            }
            if (newCompanyDto.business_reg_certificate != null)
            {
                company.business_reg_certificate = await _fileService.UploadFile(newCompanyDto.business_reg_certificate);
            }

            company.verification_status = false;
            company.registration_url= GenerateUniqueStringId(company.business_reg_no);

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailCompanyRegistration(company.company_email, company.company_name,company.registration_url); //Send Company Registration Email
            //return(company.registration_url); //Return Company Registration URL (Unique ID 
        }

        public static string GenerateUniqueStringId(int inputInteger)
        {
            while (true)
            {
                // Generate a random string of 10 characters (customize length as needed)
                string idString = GenerateRandomString(10);

                // Check if the string with the integer appended is unique
                string idWithInteger = idString + inputInteger.ToString();
                if (!seenIds.Contains(idWithInteger))
                {
                    seenIds.Add(idWithInteger); // Store generated ID for uniqueness check
                    return idString;
                }
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(charset, length)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<bool> CheckCompnayEmailExist(string Email) //Function to check company email exist
        {
            return await _context.Companies.AnyAsync(x => x.company_email == Email);
        }

        private async Task<bool> CheckCompnayRegNo(string RegNo) //Function to check company regNo exist
        {
            return await _context.Companies.AnyAsync(x => x.business_reg_no == int.Parse(RegNo));
        }

        public async Task<Company> FindByRegCheckID(string id) //Function to check company registration status
        {
            Company? company = await _context.Companies.Where(c => c.registration_url == id).FirstOrDefaultAsync();
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            return company;
        }

        public async Task Delete(int id)
        {
            Company company = await FindByID(id);

            // remove the docuemnts from the blob storage
            if (company.company_logo != null)
            {
                await _fileService.DeleteBlob(company.company_logo);
            }

            if (company.certificate_of_incorporation != null)
            {
                await _fileService.DeleteBlob(company.certificate_of_incorporation);
            }

            if (company.business_reg_certificate != null)
            {
                await _fileService.DeleteBlob(company.business_reg_certificate);
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyVerification(int companyID, CompanyRegInfoDto companyRegInfo)
        {
            var unRegCompany = await FindByID(companyID);
            string link;

            var rejectedLink = unRegCompany.registration_url;

            unRegCompany.verification_status = companyRegInfo.verification_status;
            unRegCompany.company_registered_date = companyRegInfo.company_registered_date;
            unRegCompany.verified_system_admin_id = companyRegInfo.verified_system_admin_id;
            unRegCompany.comment = companyRegInfo.comment;

            await _context.SaveChangesAsync();

            if (companyRegInfo.verification_status == true)
            {
                link = "https://polite-forest-041105700.5.azurestaticapps.net/RegCompanyAdmin?id=" + rejectedLink;
            }
            else
            {
                link = "https://polite-forest-041105700.5.azurestaticapps.net/RegCheck?id=" + rejectedLink;
            }

            _emailService.EvaluatedCompanyRegistraionApplicationEmail(unRegCompany.company_email, companyRegInfo.verification_status, companyRegInfo.comment, link, unRegCompany.company_name);
        }

        public async Task RegisterCompany(int companyID, AddDetailsCompanyDto company)
        {
            var unRegCompany = await FindByID(companyID);

            unRegCompany.company_logo = company.company_logo;
            unRegCompany.company_description = company.company_description;
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
            dbCompany.company_business_scale = company.company_business_scale;
            dbCompany.verification_status = false;
            dbCompany.company_applied_date = company.company_applied_date;
            dbCompany.comment = null;
            dbCompany.verified_system_admin_id = null;

            if (company.company_logo != null)
            {
                dbCompany.company_logo = await _fileService.UploadFile(company.company_logo);
            }
            if (company.certificate_of_incorporation != null)
            {
                dbCompany.certificate_of_incorporation = await _fileService.UploadFile(company.certificate_of_incorporation);
            }
            if (company.business_reg_certificate != null)
            {
                dbCompany.business_reg_certificate = await _fileService.UploadFile(company.business_reg_certificate);
            }

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
            dbCompany.company_description = company.company_description;
            dbCompany.company_business_scale = company.company_business_scale;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRegistered(int companyID)
        {
            Company company = await FindByID(companyID);

            return company.verification_status;
        }

        public async Task SaveCompanyLogo(IFormFile file, int companyId)
        {
            var company = await FindByID(companyId);

            if (company == null)
            {
                throw new NullReferenceException("Company not found.");
            }

            // delete the old file
            if (company.company_logo != null)
            {
                await _fileService.DeleteBlob(company.company_logo);
            }

            company.company_logo = await _fileService.UploadFile(file);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotRegisteredEligibleCompanyDto>> GetEligibleUnregisteredCompanies()
        {
          var companies = await _context.Companies
                .Where(c => c.verified_system_admin_id != null && c.company_admin_id == null)
                .Select(c => new NotRegisteredEligibleCompanyDto
                {
                    company_id = c.company_id,
                    company_name = c.company_name,
                    company_email = c.company_email,
                    company_logo = c.company_logo
                })
                .ToListAsync();

            foreach (var company in companies)
            {
                if (company.company_logo != null)
                {
                    company.company_logo = await _fileService.GetBlobUrl(company.company_logo);
                }
            }

            return companies;
        }
    }
}
