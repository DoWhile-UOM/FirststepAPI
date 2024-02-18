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
        private readonly IJobFieldService _jobFieldService;

        public AdvertisementService(
            DataContext context, 
            IMapper mapper, 
            IProfessionKeywordService keywordService, 
            IJobFieldService jobFieldService)
        {
            _context = context;
            _mapper = mapper;
            _keywordService = keywordService;
            _jobFieldService = jobFieldService;
        }

        public async Task<IEnumerable<Advertisement>> FindAll()
        {
            return await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("company")
                .ToListAsync();
        }

        public async Task<Advertisement> FindById(int id)
        {
            Advertisement? advertisement =
                await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("company")
                .FirstOrDefaultAsync(x => x.advertisement_id == id);

            if (advertisement is null)
            {
                throw new Exception("Advertisement not found.");
            }

            return advertisement;
        }
        
        public async Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID)
        {
            var advertisementList = await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Where(x => x.company_id == companyID)
                .ToListAsync();

            if (advertisementList is null)
            {
                throw new Exception("No advertisements found under this company.");
            }

            return advertisementList;
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetAll()
        {
            return MapAdsToCardDtos(await FindAll());
        }

        public async Task<AdvertisementDto> GetById(int id)
        {
            var dbAdvertismeent = await FindById(id);
            var advertisementDto = _mapper.Map<AdvertisementDto>(dbAdvertismeent);

            advertisementDto.company_name = dbAdvertismeent.company!.company_name;
            advertisementDto.field_name = dbAdvertismeent.job_Field!.field_name;

            return advertisementDto;
        }

        public async Task<IEnumerable<JobOfferDto>> GetJobOffersByCompanyID(int companyID)
        {
            var dbAdvertisements = await FindByCompanyID(companyID);

            // map to jobofferDtos
            var jobOfferDtos = new List<JobOfferDto>();

            foreach (var ad in dbAdvertisements)
            {
                var jobOfferDto = _mapper.Map<JobOfferDto>(ad);

                jobOfferDto.field_name = ad.job_Field!.field_name;

                // search number of applications
                jobOfferDto.no_of_applications = 0;

                // search number of evaluated applications
                jobOfferDto.no_of_evaluated_applications = 0;

                // search number if accepted applications
                jobOfferDto.no_of_accepted_applications = 0;

                // search number of rejected applications
                jobOfferDto.no_of_rejected_applications = 0;

                jobOfferDtos.Add(jobOfferDto);
            }

            return jobOfferDtos;
        }

        public async Task Create(AddAdvertisementDto advertisementDto)
        {
            // map the AddAdvertisementDto to a Advertisement object
            Advertisement newAdvertisement = _mapper.Map<Advertisement>(advertisementDto);

            newAdvertisement.current_status = "active";

            // find hrmanager
            var hrManager = await _context.HRManagers.FirstOrDefaultAsync(e => e.user_id == advertisementDto.hrManager_id);

            // validate hrmanager
            if (hrManager == null)
            {
                throw new Exception("HR Manager not found.");
            }

            // set the company id
            newAdvertisement.company_id = hrManager.company_id;

            newAdvertisement.job_Field = await _jobFieldService.GetById(newAdvertisement.field_id);

            // add keywords to the advertisement
            newAdvertisement.professionKeywords = await IncludeProfessionKeywordsToAdvertisement(advertisementDto.keywords, newAdvertisement.field_id);

            _context.Advertisements.Add(newAdvertisement);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int jobID, UpdateAdvertisementDto reqAdvertisement)
        {
            Advertisement dbAdvertisement = await FindById(jobID);

            dbAdvertisement.job_number = reqAdvertisement.job_number;
            dbAdvertisement.title = reqAdvertisement.title;
            dbAdvertisement.country = reqAdvertisement.country;
            dbAdvertisement.city = reqAdvertisement.city;
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

            // update keywords in the advertisement
            dbAdvertisement.professionKeywords = await IncludeProfessionKeywordsToAdvertisement(reqAdvertisement.keywords, dbAdvertisement.field_id);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Advertisement advertisement = await FindById(id);
            
            _context.Advertisements.Remove(advertisement);
            _context.SaveChanges();
        }

        // Add keywords to the advertisement
        private async Task<ICollection<ProfessionKeyword>?> IncludeProfessionKeywordsToAdvertisement(ICollection<string>? newKeywords, int fieldId)
        {
            if (newKeywords != null)
            {
                var professionKeywords = new List<ProfessionKeyword>();

                foreach (var keyword in newKeywords)
                {
                    // check whether the keyword exists in the database
                    var dbKeyword = await _keywordService.GetByName(keyword.ToLower(), fieldId);

                    if (dbKeyword != null)
                    {
                        // if it exists, add it to the advertisement's list of keywords
                        professionKeywords.Add(dbKeyword);
                    }
                    else
                    {
                        // if it doesn't exist, create a new keyword and add it to the advertisement's list of keywords
                        professionKeywords.Add(new ProfessionKeyword
                        {
                            profession_id = 0,
                            profession_name = keyword.ToLower(),
                            field_id = fieldId
                        });
                    }
                }

                return professionKeywords;
            }

            return null;
        }

        // map the advertisements to a list of AdvertisementCardDtos
        public IEnumerable<AdvertisementShortDto> MapAdsToCardDtos(IEnumerable<Advertisement> dbAds)
        {
            var adCardDtos = new List<AdvertisementShortDto>();

            foreach (var ad in dbAds)
            {
                var adDto = _mapper.Map<AdvertisementShortDto>(ad);

                adDto.company_name = ad.company!.company_name;
                adDto.field_name = ad.job_Field!.field_name;

                // recheck this part
                adDto.is_saved = false;

                adCardDtos.Add(adDto);
            }

            return adCardDtos;
        }
    }
}
