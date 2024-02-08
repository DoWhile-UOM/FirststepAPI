using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IProfessionKeywordService _keywordService;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IJobFieldService _jobFieldService;

        public AdvertisementService(
            DataContext context, 
            IMapper mapper, 
            IProfessionKeywordService keywordService, 
            ICompanyService companyService, 
            IEmployeeService employeeService, 
            IJobFieldService jobFieldService)
        {
            _context = context;
            _mapper = mapper;
            _keywordService = keywordService;
            _companyService = companyService;
            _employeeService = employeeService;
            _jobFieldService = jobFieldService;
        }

        public async Task<IEnumerable<Advertisement>> GetAll()
        {
            return await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .ToListAsync();
        }

        public async Task<Advertisement> GetById(int id)
        {
            Advertisement? advertisement = 
                await _context.Advertisements
                .Include("professionKeywords")
                .FirstOrDefaultAsync(x => x.advertisement_id == id);
            
            if (advertisement is null)
            {
                throw new Exception("Advertisement not found.");
            }

            return advertisement;
        }

        public async Task Create(AddAdvertisementDto advertisementDto)
        {
            // map the AddAdvertisementDto to a Advertisement object
            Advertisement newAdvertisement = _mapper.Map<Advertisement>(advertisementDto);

            newAdvertisement.hrManager = await _employeeService.FindHRM(newAdvertisement.hrManager_id);
            newAdvertisement.company = await _companyService.GetById(newAdvertisement.hrManager.company_id);
            newAdvertisement.company_id = newAdvertisement.company.company_id; 
            newAdvertisement.job_Field = await _jobFieldService.GetById(newAdvertisement.field_id);

            // add keywords to the advertisement
            if (advertisementDto.keywords != null)
            {
                newAdvertisement.professionKeywords = new List<ProfessionKeyword>();

                foreach (var keyword in advertisementDto.keywords)
                {
                    // check whether the keyword exists in the database
                    var dbKeyword = await _keywordService.GetByName(keyword, newAdvertisement.field_id);

                    if (dbKeyword != null)
                    {
                        // if it exists, add it to the advertisement's list of keywords
                        newAdvertisement.professionKeywords.Add(dbKeyword);
                    }
                    else
                    {
                        // if it doesn't exist, create a new keyword and add it to the advertisement's list of keywords
                        newAdvertisement.professionKeywords.Add(new ProfessionKeyword
                        {
                            profession_id = 0,
                            profession_name = keyword,
                            field_id = newAdvertisement.field_id
                        });
                    }
                }
            }

            _context.Advertisements.Add(newAdvertisement);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Advertisement advertisement)
        {
            Advertisement dbAdvertisement = await GetById(advertisement.advertisement_id);

            dbAdvertisement.job_number = advertisement.job_number;
            dbAdvertisement.title = advertisement.title;
            dbAdvertisement.location_province = advertisement.location_province;
            dbAdvertisement.location_city = advertisement.location_city;
            dbAdvertisement.employeement_type = advertisement.employeement_type;
            dbAdvertisement.arrangement = advertisement.arrangement;
            dbAdvertisement.is_experience_required = advertisement.is_experience_required;
            dbAdvertisement.salary = advertisement.salary;
            dbAdvertisement.posted_date = advertisement.posted_date;
            dbAdvertisement.submission_deadline = advertisement.submission_deadline;
            dbAdvertisement.current_status = advertisement.current_status;
            dbAdvertisement.job_overview = advertisement.job_overview;
            dbAdvertisement.job_responsibilities = advertisement.job_responsibilities;
            dbAdvertisement.job_qualifications = advertisement.job_qualifications;
            dbAdvertisement.job_benefits = advertisement.job_benefits;
            dbAdvertisement.job_other_details = advertisement.job_other_details;
            dbAdvertisement.hrManager_id = advertisement.hrManager_id;
            dbAdvertisement.field_id = advertisement.field_id;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Advertisement advertisement = await GetById(id);
            
            _context.Advertisements.Remove(advertisement);
            _context.SaveChanges();
        }

        // for find no of applications for a job
        /*
        public Task<int> findNoOfApplications(int job_id)
        {
            var dbAdvertisement = findAdvertisement(job_id);
            if (dbAdvertisement == null)
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
            //return dbAdvertisement.no_of_applications;
        }*/
    }
}
