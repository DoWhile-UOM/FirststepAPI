using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace FirstStep.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementService;
        private readonly IEmailService _emailService;//Email Service dependency injection
        private readonly IFileService _fileService;

        // Random ID generation
        private static readonly Random random = new Random();
        private static readonly HashSet<string> seenIds = new HashSet<string>();

        public CompanyService(IEmailService emailService, DataContext context, IMapper mapper, IAdvertisementService advertisementService,IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _advertisementService = advertisementService;
            _emailService=emailService;
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

        //get company list for system admin
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
            
            return advertisementCompanyDto;
        }


        //get company application by id
        public async Task<CompanyApplicationDto> GetCompanyApplicationById(int companyID)
        {
            Company company = await FindByID(companyID);
            CompanyApplicationDto companydto = _mapper.Map<CompanyApplicationDto>(company);
            return companydto;
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
            company.registration_url= GenerateUniqueStringId(company.business_reg_no);

            //Call Company Registration Email Verfication service


            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            _emailService.SendEmailCompanyRegistration(company.company_email, company.company_name,company.registration_url); //Send Company Registration Email
            //return(company.registration_url); //Return Company Registration URL (Unique ID 
        }

        //Company Resgistration State Check ID Generation Starts here
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

        //Company Resgistration State Check ID Generation Ends here


        public class EmailAlreadyExistsException : Exception
        {
            public EmailAlreadyExistsException(string message) : base(message) { }
        }

        public class RegistrationNumberAlreadyExistsException : Exception
        {
            public RegistrationNumberAlreadyExistsException(string message) : base(message) { }
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

        //-----------------Company Registration Ends here---------------------------------------------------------


        public async Task Delete(int id)
        {
            Company company = await FindByID(id);

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyVerification(int companyID, CompanyRegInfoDto companyRegInfo)
        {
            var unRegCompany = await FindByID(companyID);
            var link = "";

            var rejectedLink = unRegCompany.registration_url;

            unRegCompany.verification_status = companyRegInfo.verification_status;
            unRegCompany.company_registered_date = companyRegInfo.company_registered_date;
            unRegCompany.verified_system_admin_id = companyRegInfo.verified_system_admin_id;
            unRegCompany.comment = companyRegInfo.comment;

            await _context.SaveChangesAsync();

            if (companyRegInfo.verification_status == true)
            {
                link = "http://localhost:4200/RegCompanyAdmin?id=" + rejectedLink;
            }
            else
            {
                link = "http://localhost:4200/RegCheck?id=" + rejectedLink;
            }
            _emailService.EvaluatedCompanyRegistraionApplicationEmail(unRegCompany.company_email, companyRegInfo.verification_status, companyRegInfo.comment, link, unRegCompany.company_name);

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
        /*  public async Task<bool> SaveCompanyLogo(IFormFile file, int companyId)
          {
              var response = await _fileService.UploadFiles(new List<IFormFile> { file });
              if(response == null || response.Count == 0)
              {
                  return false;
              }
              // Get the URL of the uploaded file
              var blobName = file.FileName;
              var fileUrl = await _fileService.GetBlobImageUrl(blobName);

              // Save the file information in the database
              var company = await FindByID(companyId);
              if(company == null)
              {
                  return false;
              }
              company.company_logo= fileUrl;

              await _context.SaveChangesAsync();

              return true;

          }*/
        public async Task<bool> SaveCompanyLogo(IFormFile file, int companyId)
        {
            var logoBlobName = await _fileService.UploadFile(file);
            if (logoBlobName == null)
            {
                return false;
            }
            
            // Save the file information in the database
            var company = await FindByID(companyId);
            if (company == null)
            {
                return false;
            }
            company.company_logo = logoBlobName;

            await _context.SaveChangesAsync();

            return true;

        }
    }
}
