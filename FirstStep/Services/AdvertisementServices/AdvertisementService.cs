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
                    var dbKeyword = await _keywordService.GetByName(keyword.ToLower(), newAdvertisement.field_id);

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
                            profession_name = keyword.ToLower(),
                            field_id = newAdvertisement.field_id
                        });
                    }
                }
            }

            _context.Advertisements.Add(newAdvertisement);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int jobID, UpdateAdvertisementDto reqAdvertisement)
        {
            Advertisement dbAdvertisement = await GetById(jobID);

            dbAdvertisement.job_number = reqAdvertisement.job_number;
            dbAdvertisement.title = reqAdvertisement.title;
            dbAdvertisement.location_province = reqAdvertisement.location_province;
            dbAdvertisement.location_city = reqAdvertisement.location_city;
            dbAdvertisement.employeement_type = reqAdvertisement.employeement_type;
            dbAdvertisement.arrangement = reqAdvertisement.arrangement;
            dbAdvertisement.is_experience_required = reqAdvertisement.is_experience_required;
            dbAdvertisement.salary = reqAdvertisement.salary;
            dbAdvertisement.submission_deadline = reqAdvertisement.submission_deadline;
            dbAdvertisement.job_overview = reqAdvertisement.job_overview;
            dbAdvertisement.job_responsibilities = reqAdvertisement.job_responsibilities;
            dbAdvertisement.job_qualifications = reqAdvertisement.job_qualifications;
            dbAdvertisement.job_benefits = reqAdvertisement.job_benefits;
            dbAdvertisement.job_other_details = reqAdvertisement.job_other_details;
            dbAdvertisement.field_id = reqAdvertisement.field_id;

            // add keywords to the advertisement
            if (reqAdvertisement.keywords != null)
            {
                foreach (var keyword in reqAdvertisement.keywords)
                {
                    // check whether the keyword exists in the database
                    var dbKeyword = await _keywordService.GetByName(keyword.ToLower(), dbAdvertisement.field_id);

                    // create new profession keyword array when it is null
                    if (dbAdvertisement.professionKeywords == null)
                    {
                        dbAdvertisement.professionKeywords = new List<ProfessionKeyword>();
                    }

                    if (dbKeyword != null && !dbAdvertisement.professionKeywords.Contains(dbKeyword))
                    {
                        // if it exists, check whether it is already in the list
                        // if it is not, add it to the advertisement's list of keywords
                        dbAdvertisement.professionKeywords.Add(dbKeyword);
                    }
                    else if (dbKeyword == null)
                    {
                        // if it doesn't exist, create a new keyword and add it to the advertisement's list of keywords
                        dbAdvertisement.professionKeywords.Add(new ProfessionKeyword
                        {
                            profession_id = 0,
                            profession_name = keyword.ToLower(),
                            field_id = dbAdvertisement.field_id
                        });
                    }
                }
            }

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
