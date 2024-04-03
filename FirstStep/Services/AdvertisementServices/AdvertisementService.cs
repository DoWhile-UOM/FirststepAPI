using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using KdTree;
using KdTree.Math;

namespace FirstStep.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IProfessionKeywordService _keywordService;
        private readonly ISkillService _skillService;
        private readonly ISeekerService _seekerService;
        private readonly IApplicationService _applicationService;

        public AdvertisementService(
            DataContext context, 
            IMapper mapper,
            IProfessionKeywordService keywordService,
            ISkillService skillService,
            ISeekerService seekerService,
            IApplicationService applicationService)
        {
            _context = context;
            _mapper = mapper;
            _keywordService = keywordService;
            _skillService = skillService;
            _seekerService = seekerService;
            _applicationService = applicationService;
        }

        enum AdvertisementStatus { active, closed }

        public async Task<IEnumerable<Advertisement>> FindAll()
        {
            // get all active advertisements
            return await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("hrManager")
                .Include("skills")
                .Include("savedSeekers")
                .Where(x => x.current_status == AdvertisementStatus.active.ToString())
                .ToListAsync();
        }

        public async Task<Advertisement> FindById(int id)
        {
            Advertisement? advertisement =
                await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("hrManager")
                .Include("skills")
                .Include("savedSeekers")
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
                .Include("hrManager")
                .Include("skills")
                .Include("savedSeekers")
                .Where(x => x.hrManager!.company_id == companyID)
                .ToListAsync();

            if (advertisementList is null)
            {
                throw new Exception("No advertisements found under this company.");
            }

            return advertisementList;
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetAll(int seekerID)
        {
            return await CreateAdvertisementList(await FindAll(), seekerID);
        }

        public async Task<AdvertisementDto> GetById(int id)
        {
            var dbAdvertismeent = await FindById(id);
            var advertisementDto = _mapper.Map<AdvertisementDto>(dbAdvertismeent);

            advertisementDto.company_name = _context.Companies.Find(dbAdvertismeent.hrManager!.company_id)!.company_name;

            return advertisementDto;
        }

        // fill the advertisement form when updating an advertisement
        public async Task<UpdateAdvertisementDto> GetByIdWithKeywords(int id)
        {
            var dbAdvertismeent = await FindById(id);
            var currentAdData = _mapper.Map<UpdateAdvertisementDto>(dbAdvertismeent);
               
            currentAdData.reqSkills = dbAdvertismeent.skills!.Select(e => e.skill_name).ToList();
            currentAdData.reqKeywords = dbAdvertismeent.professionKeywords!.Select(e => e.profession_name).ToList();

            return currentAdData;
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetAdvertisementsByCompany(int companyID, string status, string title)
        {
            ValidateStatus(status);

            if (title != null)
            {

                var dbAdvertisements = await FindByCompanyID(companyID);
                
                // filter advertisements by title
                var filteredAdvertisements = dbAdvertisements.Where(x => x.title.ToLower().Contains(title.ToLower())).ToList();
                
                // split title into sub parts and filter advertisements by each sub part
                List<string> titleSubParts = title.Split(' ').ToList();
                foreach (string subPart in titleSubParts)
                {
                    foreach (var ad in dbAdvertisements.Where(x => x.title.ToLower().Contains(subPart.ToLower())).ToList())
                    {
                        if (!filteredAdvertisements.Contains(ad))
                        {
                            filteredAdvertisements.Add(ad);
                        }
                    }
                }

                return await CreateAdvertisementList(filteredAdvertisements, status);
            }
            else
            {
                return await GetAdvertisementsByCompany(companyID, status);
            }
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetAdvertisementsByCompany(int companyID, string status)
        {
            ValidateStatus(status);

            var dbAdvertisements = await FindByCompanyID(companyID);

            return await CreateAdvertisementList(dbAdvertisements, status);
        }

        public async Task Create(AddAdvertisementDto advertisementDto)
        {
            // validate hrManagerID
            if (await _context.HRManagers.FindAsync(advertisementDto.hrManager_id) is null)
            {
                throw new Exception("Invalid HR Manager ID.");
            }

            // validate job field id
            if (await _context.JobFields.FindAsync(advertisementDto.field_id) is null)
            {
                throw new Exception("Invalid job field ID.");
            }
            
            // map the AddAdvertisementDto to a Advertisement object
            Advertisement newAdvertisement = _mapper.Map<Advertisement>(advertisementDto);

            // add keywords to the advertisement
            newAdvertisement.professionKeywords = await IncludeKeywordsToAdvertisement(advertisementDto.keywords, newAdvertisement.field_id);
            
            // add skills to the advertisement
            newAdvertisement.skills = await IncludeSkillsToAdvertisement(advertisementDto.reqSkills);

            newAdvertisement.current_status = AdvertisementStatus.active.ToString();
            _context.Advertisements.Add(newAdvertisement);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatus(int id, string newStatus)
        {
            ValidateStatus(newStatus);
            var advertisement = await FindById(id);

            advertisement.current_status = newStatus;
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
            dbAdvertisement.job_description = reqAdvertisement.job_description;
            dbAdvertisement.field_id = reqAdvertisement.field_id;

            // update keywords in the advertisement
            dbAdvertisement.professionKeywords = await IncludeKeywordsToAdvertisement(reqAdvertisement.reqKeywords, dbAdvertisement.field_id);
            
            // update skills in the advertisement
            dbAdvertisement.skills = await IncludeSkillsToAdvertisement(reqAdvertisement.reqSkills);

            await _context.SaveChangesAsync();
        }

        public async Task SaveAdvertisement(int advertisementId, int seekerId, bool isSave)
        {
            // find the seeker
            var seeker = await _seekerService.GetById(seekerId);

            // find the advertisement
            var advertisement = await FindById(advertisementId);

            // check whether the advertisement is already saved
            if (advertisement.savedSeekers is null)
            {
                advertisement.savedSeekers = new List<Seeker>();
            }
            
            if (!advertisement.savedSeekers.Contains(seeker) && isSave)
            {
                // add the seeker to the list of saved seekers
                advertisement.savedSeekers.Add(seeker);
                await _context.SaveChangesAsync();
            }
            else if (advertisement.savedSeekers.Contains(seeker) && !isSave)
            {
                // remove the seeker from the list of saved seekers
                advertisement.savedSeekers.Remove(seeker);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetSavedAdvertisements(int seekerID)
        {
            var advertisements = await FindAll();

            var savedAds = new List<Advertisement>();

            foreach (var ad in advertisements)
            {
                if (ad.savedSeekers != null && ad.savedSeekers.Any(e => e.user_id == seekerID))
                {
                    savedAds.Add(ad);
                }
            }

            return await CreateAdvertisementList(savedAds, 0);
        }

        private async Task<bool> IsAdvertisementSaved(int advertisementId, int seekerId)
        {
            // find the seeker
            var seeker = await _seekerService.GetById(seekerId);

            // find the advertisement
            var advertisement = await FindById(advertisementId);

            if (advertisement.savedSeekers is null || !advertisement.savedSeekers.Contains(seeker))
            {
                return false;
            }

            return true;
        }

        public async Task Delete(int id)
        {
            Advertisement advertisement = await FindById(id);
            
            _context.Advertisements.Remove(advertisement);
            _context.SaveChanges();
        }

        // Add keywords to the advertisement
        private async Task<ICollection<ProfessionKeyword>?> IncludeKeywordsToAdvertisement(ICollection<string>? newKeywords, int fieldId)
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

        // Add skills to the advertisement
        private async Task<ICollection<Skill>?> IncludeSkillsToAdvertisement(ICollection<string>? newSkills)
        {
            if (newSkills != null)
            {
                var skills = new List<Skill>();

                foreach (var skill in newSkills)
                {
                    // check whether the skill exists in the database
                    var dbSkill = await _skillService.GetByName(skill.ToLower());

                    if (dbSkill != null)
                    {
                        // if it exists, add it to the advertisement's list of skills
                        skills.Add(dbSkill);
                    }
                    else
                    {
                        // if it doesn't exist, create a new skill and add it to the advertisement's list of skills
                        skills.Add(new Skill
                        {
                            skill_id = 0,
                            skill_name = skill.ToLower()
                        });
                    }                    
                }

                return skills;
            }

            return null;
        }

        public async Task<AdvertisementFirstPageDto> GetFirstPage(int seekerID, int noOfresultsPerPage)
        {
            var dbAds = await FindAll();

            return await CreateFirstPageResults(dbAds, seekerID, noOfresultsPerPage);
        }

        private async Task<AdvertisementFirstPageDto> CreateFirstPageResults(IEnumerable<Advertisement> dbAds, int seekerID, int noOfresultsPerPage)
        {
            AdvertisementFirstPageDto firstPageResults = new AdvertisementFirstPageDto();

            // select only number of advertisements per page
            firstPageResults.FirstPageAdvertisements = await CreateAdvertisementList(dbAds.Take(noOfresultsPerPage), seekerID);

            // add all advertisement ids into a list
            firstPageResults.allAdvertisementIds = dbAds.Select(e => e.advertisement_id).ToList();

            return firstPageResults;
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID)
        {
            var advertisements = new List<Advertisement>();

            foreach (var adId in adList)
            {
                advertisements.Add(await FindById(adId));
            }

            return await CreateAdvertisementList(advertisements, seekerID);
        }

        // map the advertisements to a list of AdvertisementCardDtos and create advertisement list for the seeker
        public async Task<IEnumerable<AdvertisementShortDto>> CreateAdvertisementList(IEnumerable<Advertisement> dbAds, int seekerID)
        {
            var adCardDtos = new List<AdvertisementShortDto>();

            foreach (var ad in dbAds)
            {
                var adDto = _mapper.Map<AdvertisementShortDto>(ad);

                adDto.company_name = _context.Companies.Find(ad.hrManager!.company_id)!.company_name;
                // when seekerID is 0, it means that the all advertisements are saved by the seeker
                // from GetSavedAdvertisements method passed seekerID as 0
                if (seekerID != 0)
                {
                    // check whether the advertisement is saved by the seeker
                    adDto.is_saved = await IsAdvertisementSaved(ad.advertisement_id, seekerID);
                }
                else
                {
                    adDto.is_saved = true;
                }

                adCardDtos.Add(adDto);
            }

            return adCardDtos;
        }

        // map the advertisements to a list of JobOfferDtos and create advertisement list for the company
        public async Task<IEnumerable<AdvertisementTableRowDto>> CreateAdvertisementList(IEnumerable<Advertisement> dbAds, string status)
        {
            // map to jobofferDtos
            var jobOfferDtos = new List<AdvertisementTableRowDto>();

            foreach (var ad in dbAds)
            {
                var jobOfferDto = _mapper.Map<AdvertisementTableRowDto>(ad);

                if (status != "all" && ad.current_status != status)
                {
                    continue;
                }

                jobOfferDto.field_name = ad.job_Field!.field_name;

                // search number of applications
                jobOfferDto.no_of_applications = await _applicationService.NumberOfApplicationsByAdvertisementId(ad.advertisement_id);

                // search number of evaluated applications
                jobOfferDto.no_of_evaluated_applications = await _applicationService.TotalNotEvaluatedApplications(ad.advertisement_id);

                // search number if accepted applications
                jobOfferDto.no_of_accepted_applications = await _applicationService.AcceptedApplications(ad.advertisement_id);

                // search number of rejected applications
                jobOfferDto.no_of_rejected_applications = await _applicationService.RejectedApplications(ad.advertisement_id);

                jobOfferDtos.Add(jobOfferDto);
            }

            return jobOfferDtos;
        }

        // validate status
        private void ValidateStatus(string status)
        {
            var possibleStatuses = new List<string> 
            { 
                AdvertisementStatus.active.ToString(), 
                AdvertisementStatus.closed.ToString(), 
                "all" 
            };

            if (!possibleStatuses.Contains(status))
            {
                throw new Exception("Invalid status.");
            }
        }

        public async Task<AdvertisementFirstPageDto> BasicSearch(SearchJobRequestDto requestAdsDto, int seekerID, int pageLength)
        {
            var advertisements = await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("skills")
                .Include("hrManager")
                .Include("savedSeekers")
                .Where(ad =>
                    ad.country == requestAdsDto.country &&
                    ad.city == requestAdsDto.city &&
                        (ad.arrangement == requestAdsDto.arrangement ||
                        ad.employeement_type == requestAdsDto.employeement_type) &&
                    ad.current_status == AdvertisementStatus.active.ToString())
                .ToListAsync();

            if (requestAdsDto.title == null)
            {
                return await CreateFirstPageResults(advertisements, seekerID, pageLength);
            }

            var filteredAdvertisements = new List<Advertisement> { };

            filteredAdvertisements
                .AddRange(advertisements
                    .Where(ad => ad.title.ToLower().Contains(requestAdsDto.title.ToLower()))
                    .ToList());

            var titles = requestAdsDto.title.Split(' ');

            foreach (var title in titles)
            {
                var ads = new List<Advertisement> { };

                ads.AddRange(advertisements
                    .Where(ad => ad.title.ToLower().Contains(title.ToLower()))
                    .ToList());

                for (int i = 0; i < ads.Count; i++)
                {
                    if (!filteredAdvertisements.Contains(ads[i]))
                    {
                        filteredAdvertisements.Add(ads[i]);
                    }
                }
            }

            foreach (var title in titles)
            {
                foreach (var ad in advertisements)
                {
                    if (!filteredAdvertisements.Contains(ad) && ad.professionKeywords != null)
                    {
                        var keywords = ad.professionKeywords.Select(e => e.profession_name).ToList();

                        if (keywords.Contains(title.ToLower()))
                        {
                            filteredAdvertisements.Add(ad);
                        }
                    }
                }
            }

            return await CreateFirstPageResults(filteredAdvertisements, seekerID, pageLength);
        }

        public async Task<IEnumerable<AdvertisementShortDto>> AdvanceSearch(SearchJobRequestDto requestAdsDto, int seekerID)
        {
            var advertisements = await FindAll();

            // convert advertisements into kdtree
            var tree = new KdTree<float, Advertisement>(5, new FloatMath());

            foreach (var ad in advertisements)
            {
                // Add each advertisement to the k-d tree
                var key = new[]
                {
                    (float)ad.title.GetHashCode(),
                    (float)ad.country.GetHashCode(),
                    (float)ad.city.GetHashCode(),
                    (float)ad.employeement_type.GetHashCode(),
                    (float)ad.arrangement.GetHashCode()
                };

                tree.Add(key, ad);
            }
            
            var userRequest = new[]
            {
                (float)requestAdsDto.title!.GetHashCode(),
                (float)requestAdsDto.country!.GetHashCode(),
                (float)requestAdsDto.city!.GetHashCode(),
                (float)requestAdsDto.employeement_type!.GetHashCode(),
                (float)requestAdsDto.arrangement!.GetHashCode()
            };

            var nearestAds = tree.GetNearestNeighbours(userRequest, 10);

            //var closestAd = nearestAds.FirstOrDefault()?.Value;

            var filteredAdvertisements = nearestAds.Select(e => e.Value).ToList();

            return await CreateAdvertisementList(filteredAdvertisements, seekerID);
        }
    }
}
