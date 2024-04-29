using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Helper;
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

        private readonly int AdvertisementExpiredDays = 10;
        private enum AdvertisementStatus { active, closed }

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

        private async Task<IEnumerable<Advertisement>> FindAll(bool isActivatedOnly)
        {
            if (isActivatedOnly)
            {
                return await _context.Advertisements
                    .Include("professionKeywords")
                    .Include("job_Field")
                    .Include("hrManager")
                    .Include("skills")
                    .Include("savedSeekers")
                    .Where(x => x.current_status == AdvertisementStatus.active.ToString())
                    .ToListAsync();
            }
            else
            {
                return await _context.Advertisements
                    .Include("professionKeywords")
                    .Include("job_Field")
                    .Include("hrManager")
                    .Include("skills")
                    .Include("savedSeekers")
                    .ToListAsync();
            }
        }

        private async Task<Advertisement> FindById(int id)
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
                throw new NullReferenceException("Advertisement not found.");
            }

            return advertisement;
        }
        
        private async Task<IEnumerable<Advertisement>> FindByCompanyID(int companyID)
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
                throw new NullReferenceException("No advertisements found under this company.");
            }

            return advertisementList;
        }

        private async Task<IEnumerable<Advertisement>> FindBySeekerJobFieldID(int seekerID)
        {
            // get all active advertisements
            IEnumerable<Advertisement> advertisements = await FindAll(true);

            // find the seeker's field
            JobField seekerField = await _seekerService.GetSeekerField(seekerID);

            // filter advertisements by seeker's field
            return advertisements.Where(x => x.field_id == seekerField.field_id).ToList();
        }

        public async Task<AdvertisementFirstPageDto> GetAllWithPages(int seekerID, int noOfresultsPerPage)
        {
            var dbAds = await FindBySeekerJobFieldID(seekerID);

            return await CreateFirstPageResults(dbAds, seekerID, noOfresultsPerPage);
        }

        public async Task<AdvertisementDto> GetById(int id)
        {
            var dbAdvertismeent = await FindById(id);
            var advertisementDto = _mapper.Map<AdvertisementDto>(dbAdvertismeent);

            advertisementDto.company_name = _context.Companies.Find(dbAdvertismeent.hrManager!.company_id)!.company_name;
            advertisementDto.is_expired = IsExpired(dbAdvertismeent);

            return advertisementDto;
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID)
        {
            var advertisements = new List<Advertisement>();

            foreach (var adId in adList)
            {
                advertisements.Add(await FindById(adId));
            }

            return await CreateAdvertisementList(advertisements, seekerID, false);
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

        public async Task<IEnumerable<Advertisement>> GetByCompanyID(int companyID)
        {
            var advertisements = await FindByCompanyID(companyID);

            return advertisements.Where(e => e.current_status == AdvertisementStatus.active.ToString()).ToList();
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetByCompanyID(int companyID, string status)
        {
            ValidateStatus(status);

            var dbAdvertisements = await FindByCompanyID(companyID);

            return await CreateAdvertisementList(dbAdvertisements, status);
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetByCompanyID(int companyID, string status, string title)
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
                return await GetByCompanyID(companyID, status);
            }
        }

        public async Task<AdvertisementFirstPageDto> GetFirstPage(int seekerID, int noOfResultsPerPage)
        {
            var matchingAds = await FindMatchingAdvertisement(seekerID);

            return await CreateFirstPageResults(matchingAds, seekerID, noOfResultsPerPage);
        }

        public async Task<AdvertisementFirstPageDto> GetFirstPage(int seekerID, string city, int noOfResultsPerPage)
        {
            if (city == null || city == "")
            {
                return await GetFirstPage(seekerID, noOfResultsPerPage);
            }

            var matchingAds = await FindMatchingAdvertisement(seekerID, city);

            return await CreateFirstPageResults(matchingAds, seekerID, noOfResultsPerPage);
        }

        public async Task Create(AddAdvertisementDto advertisementDto)
        {
            // validate hrManagerID
            if (await _context.HRManagers.FindAsync(advertisementDto.hrManager_id) is null)
            {
                throw new InvalidDataException("Invalid HR Manager ID.");
            }

            // validate job field id
            if (await _context.JobFields.FindAsync(advertisementDto.field_id) is null)
            {
                throw new InvalidDataException("Invalid job field ID.");
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

            if (newStatus == AdvertisementStatus.active.ToString() && IsExpired(advertisement))
            {
                // can't activate an expired advertisement, therefore first need to update the submission deadline
                throw new InvalidDataException("Cannot activate an expired advertisement.");
            }

            // update the advertisement status
            advertisement.current_status = newStatus;

            if (newStatus == AdvertisementStatus.closed.ToString())
            {
                // set the expired date to the current date
                advertisement.expired_date = DateTime.Now.AddDays(AdvertisementExpiredDays);
            }
            else
            {
                advertisement.expired_date = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Update(int jobID, UpdateAdvertisementDto reqAdvertisement)
        {
            Advertisement dbAdvertisement = await FindById(jobID);

            if (reqAdvertisement.submission_deadline > dbAdvertisement.submission_deadline)
            {
                dbAdvertisement.expired_date = null;
            }

            dbAdvertisement.job_number = reqAdvertisement.job_number;
            dbAdvertisement.title = reqAdvertisement.title;
            dbAdvertisement.country = reqAdvertisement.country;
            dbAdvertisement.city = reqAdvertisement.city;
            dbAdvertisement.employeement_type = reqAdvertisement.employeement_type;
            dbAdvertisement.arrangement = reqAdvertisement.arrangement;
            dbAdvertisement.experience = reqAdvertisement.experience;
            dbAdvertisement.salary = reqAdvertisement.salary;
            dbAdvertisement.currency_unit = reqAdvertisement.currency_unit;
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
            // get all advertisements (with closed advrtisements)
            var advertisements = await FindAll(false);

            return await CreateAdvertisementList(advertisements, seekerID, true);
        }

        
        public async Task<IEnumerable<AppliedAdvertisementShortDto>> GetAppliedAdvertisements(int seekerID)
        {
            Seeker seeker = await _seekerService.GetById(seekerID);

            var appliedAdvertismentList = new List<AppliedAdvertisementShortDto>();

            // find all the applications that send by the seeker
            var submittedApplications = await _applicationService.GetBySeekerId(seeker.user_id);

            foreach (var submitApplication in submittedApplications)
            {
                var dbAdvertisement = await FindById(submitApplication.advertisement_id);
                var appliedAdvertisement = _mapper.Map<AppliedAdvertisementShortDto>(dbAdvertisement);

                // find the current application status by checking the last revision for the application
                appliedAdvertisement.application_status = _applicationService.GetCurrentApplicationStatus(submitApplication);

                appliedAdvertisement.application_id = submitApplication.application_Id;
                appliedAdvertisement.company_name = _context.Companies.Find(dbAdvertisement.hrManager!.company_id)!.company_name;

                appliedAdvertismentList.Add(appliedAdvertisement);
            }

            return appliedAdvertismentList;
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

        public async Task<AdvertisementFirstPageDto> CreateFirstPageResults(IEnumerable<Advertisement> dbAds, int seekerID, int noOfresultsPerPage)
        {
            AdvertisementFirstPageDto firstPageResults = new AdvertisementFirstPageDto();

            // select only number of advertisements per page
            firstPageResults.FirstPageAdvertisements = await CreateAdvertisementList(dbAds.Take(noOfresultsPerPage), seekerID, false);

            // add all advertisement ids into a list
            firstPageResults.allAdvertisementIds = dbAds.Select(e => e.advertisement_id).ToList();

            return firstPageResults;
        }

        // map the advertisements to a list of AdvertisementCardDtos and create advertisement list for the seeker
        private async Task<IEnumerable<AdvertisementShortDto>> CreateAdvertisementList(IEnumerable<Advertisement> dbAds, int seekerID, bool isSaveOnly)
        {
            var adCardDtos = new List<AdvertisementShortDto>();

            Seeker seeker = await _seekerService.GetById(seekerID);

            foreach (var ad in dbAds)
            {
                var adDto = _mapper.Map<AdvertisementShortDto>(ad);

                // check whether the advertisement is saved by the seeker
                adDto.is_saved = IsSaved(ad, seeker);
                if (isSaveOnly && !adDto.is_saved) continue;

                // check whether the advertisement is expired or not
                adDto.is_expired = IsExpired(ad);

                adDto.company_name = _context.Companies.Find(ad.hrManager!.company_id)!.company_name;

                adCardDtos.Add(adDto);
            }

            return adCardDtos;
        }

        // map the advertisements to a list of JobOfferDtos and create advertisement list for the company
        private async Task<IEnumerable<AdvertisementTableRowDto>> CreateAdvertisementList(IEnumerable<Advertisement> dbAds, string status)
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

                jobOfferDto.no_of_applications = await _applicationService.NumberOfApplicationsByAdvertisementId(ad.advertisement_id);
                jobOfferDto.no_of_evaluated_applications = await _applicationService.TotalNotEvaluatedApplications(ad.advertisement_id);
                jobOfferDto.no_of_accepted_applications = await _applicationService.AcceptedApplications(ad.advertisement_id);
                jobOfferDto.no_of_rejected_applications = await _applicationService.RejectedApplications(ad.advertisement_id);

                jobOfferDtos.Add(jobOfferDto);
            }

            return jobOfferDtos;
        }

        public async Task CreateApplication(AddApplicationDto newApplication)
        {
            var advertisement = await FindById(newApplication.advertisement_id);

            if (!IsActive(advertisement))
            {
                throw new InvalidDataException("Cannot apply to a closed advertisement.");
            }

            if (IsExpired(advertisement))
            {
                throw new InvalidDataException("Cannot apply to an expired advertisement.");
            }

            if (!(await _seekerService.IsValidSeeker(newApplication.seeker_id)))
            {
                throw new NullReferenceException("Can't find the seeker");
            }

            await _applicationService.Create(newApplication);
        }

        public async Task<AdvertisementFirstPageDto> BasicSearch(SearchJobRequestDto requestAdsDto, int seekerID, int pageLength)
        {
            // validate and find the seeker's field
            JobField reqField = await _seekerService.GetSeekerField(seekerID);

            // get all active advertisements with filtering by country and field
            List<Advertisement> advertisements = await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("hrManager")
                .Include("savedSeekers")
                .Where(ad =>
                    ad.current_status == AdvertisementStatus.active.ToString() &&
                    ad.country == requestAdsDto.country &&
                    ad.field_id == reqField.field_id)
                .ToListAsync();

            List<Advertisement> filteredAdvertisements = new List<Advertisement> { };

            // filter advertisements by arrangement
            if (requestAdsDto.arrangement != null)
            {
                filteredAdvertisements.AddRange(advertisements
                    .Where(ad => requestAdsDto.arrangement.Contains(ad.arrangement))
                    .ToList());
            }

            // filter advertisements by employeement type
            if (requestAdsDto.employeement_type != null)
            {
                var ads = advertisements
                    .Where(ad => requestAdsDto.employeement_type.Contains(ad.employeement_type))
                    .ToList();

                // check whether the advertisement is already in the list
                foreach (var ad in ads)
                {
                    if (!filteredAdvertisements.Contains(ad))
                    {
                        filteredAdvertisements.Add(ad);
                    }
                }
            }

            // filter advertisements by title
            filteredAdvertisements = FilterByTitle(filteredAdvertisements, requestAdsDto.title);

            // filter advertisements by city
            filteredAdvertisements = await FilterByCity(filteredAdvertisements, requestAdsDto.city, requestAdsDto.distance);

            return await CreateFirstPageResults(filteredAdvertisements, seekerID, pageLength);
        }

        private List<Advertisement> FilterByTitle(List<Advertisement> advertisements, string? reqTitle)
        {
            if (reqTitle == null)
            {
                return advertisements;
            }

            var filteredAdvertisements = new List<Advertisement> { };

            filteredAdvertisements
                .AddRange(advertisements
                    .Where(ad => ad.title.ToLower().Contains(reqTitle.ToLower()))
                    .ToList());

            var titles = reqTitle.Split(' ');

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

            return filteredAdvertisements;
        }

        private async Task<List<Advertisement>> FilterByCity(List<Advertisement> advertisements, string? reqCity, float? reqDistance)
        {
            if (reqCity == null || reqDistance < 0)
            {
                return advertisements;
            }

            // reqDistance is 0, means that the seeker wants to search only in the requested city
            if (reqDistance == 0)
            {
                return advertisements
                    .Where(ad => ad.city.ToLower() == reqCity.ToLower())
                    .ToList();
            }

            var filteredAdvertisements = new List<Advertisement> { };

            // get coordinates of the requested city
            var reqCityCoordinate = await MapAPI.GetCoordinates(reqCity.ToLower());

            foreach (Advertisement advertisement in advertisements)
            {
                if (await MapAPI.GetDistance(advertisement.city, reqCityCoordinate) <= reqDistance)
                {
                    filteredAdvertisements.Add(advertisement);
                }
            }

            return filteredAdvertisements;
        }

        private async Task<List<Advertisement>> FindMatchingAdvertisement(int seekerID)
        {
            var seeker = await _seekerService.GetById(seekerID);

            // get all active advertisements in seeker's field
            var advertisements = await FindBySeekerJobFieldID(seekerID);

            if (seeker.skills == null)
            {
                // when seeker has no skills, return all advertisements in the seeker's field
                return advertisements.ToList();
            }

            // hold advertisements that match with the seeker's skills
            Dictionary<Advertisement, int> matchingAdvertisements = FindAdvertisementsMatchingWithSkills(seeker, advertisements);

            return matchingAdvertisements.Keys.ToList();
        }

        private async Task<List<Advertisement>> FindMatchingAdvertisement(int seekerID, string city)
        {
            var seeker = await _seekerService.GetById(seekerID);

            // get all active advertisements in seeker's field
            var advertisements = await FindBySeekerJobFieldID(seekerID);
            
            if (seeker.skills == null)
            {
                // when seeker has no skills, return all advertisements in the seeker's field
                return advertisements.ToList();
            }

            // find advertisements matching with the seeker's skills
            Dictionary<Advertisement, int> filteredAds = FindAdvertisementsMatchingWithSkills(seeker, advertisements);

            if (city == "")
            {
                return filteredAds.Keys.ToList();
            }

            // find the distance between the seeker's city and the advertisement's city
            Dictionary<int, float> advertisementDistances = await FindAdvertisementsMatchingWithDistance(city, filteredAds.Keys);

            // calculate the mean of the matching skills and distances
            int meanSkills = filteredAds.Values.Sum() / filteredAds.Count;
            float meanDistance = advertisementDistances.Values.Sum() / advertisementDistances.Count;

            // select only advertisements that have greater than the mean of the matching skills
            filteredAds = filteredAds.Where(e => e.Value >= meanSkills).ToDictionary(e => e.Key, e => e.Value);

            // select only advertisements that have less than the mean of the distance
            advertisementDistances = advertisementDistances.Where(e => e.Value <= meanDistance).ToDictionary(e => e.Key, e => e.Value);

            // find the common advertisements between the two dictionaries
            var matchingAdvertisements = new Dictionary<Advertisement, (int noOfSkills, float distance) > { };

            foreach (var ad in filteredAds)
            {
                if (advertisementDistances.ContainsKey(ad.Key.advertisement_id))
                {
                    matchingAdvertisements.Add(ad.Key, (ad.Value, advertisementDistances[ad.Key.advertisement_id]));
                }
            }

            return matchingAdvertisements.Keys.ToList();
        }

        private async Task<Dictionary<int, float>> FindAdvertisementsMatchingWithDistance(string city, IEnumerable<Advertisement> advertisements)
        {
            // get distance from the seeker's city to matching advertisements' cities
            Coordinate seekerCityCoordinate = await MapAPI.GetCoordinates(city.ToLower());

            // hold advertisements that match with the seeker's skills and distance
            Dictionary<int, float> advertisementDistances = new Dictionary<int, float>();

            // recent calculated distances
            Dictionary<string, Coordinate> recentCalculatedDistances = new Dictionary<string, Coordinate>();

            Coordinate adCityCoordinate;

            // calculate the distance between the seeker's city and the advertisement's city
            foreach (var ad in advertisements)
            {
                if (recentCalculatedDistances.ContainsKey(ad.city.ToLower()))
                {
                    adCityCoordinate = await MapAPI.GetCoordinates(ad.city.ToLower());
                    recentCalculatedDistances.Add(ad.city.ToLower(), adCityCoordinate);
                }
                else
                {
                    adCityCoordinate = recentCalculatedDistances[ad.city.ToLower()];
                }

                advertisementDistances.Add(ad.advertisement_id, MapAPI.GetDistance(seekerCityCoordinate, adCityCoordinate));
            }

            return advertisementDistances;
        }

        private Dictionary<Advertisement, int> FindAdvertisementsMatchingWithSkills(Seeker seeker, IEnumerable<Advertisement> advertisements)
        {
            Dictionary<Advertisement, int> matchingAdvertisements = new Dictionary<Advertisement, int>();

            // find the seeker's skills
            var seekerSkills = seeker.skills!.Select(e => e.skill_name).ToList();

            // count and add advertisements that match the seeker's skills
            foreach (var ad in advertisements)
            {
                var adSkills = ad.skills!.Select(e => e.skill_name).ToList();

                int matchingSkills = 0;

                foreach (var skill in seekerSkills)
                {
                    if (adSkills.Contains(skill))
                    {
                        matchingSkills++;
                    }
                }

                if (matchingSkills > 0)
                {
                    matchingAdvertisements.Add(ad, matchingSkills);
                }
            }

            // sort by a number of matching skills in acending order
            matchingAdvertisements = matchingAdvertisements.OrderBy(e => e.Value).ToDictionary(e => e.Key, e => e.Value);

            return matchingAdvertisements;
        }

        public async Task CloseExpiredAdvertisements()
        {
            // get all active advertisements
            var advertisements = await FindAll(true);

            foreach (var ad in advertisements)
            {
                if (ad.submission_deadline == null) continue;

                if (DateTime.Now > ad.submission_deadline)
                {
                    ad.current_status = AdvertisementStatus.closed.ToString();
                    ad.expired_date = DateTime.Now.AddDays(AdvertisementExpiredDays);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveSavedExpiredAdvertisements()
        {
            var advertisements = await FindAll(false);

            foreach (var ad in advertisements)
            {
                if (ad.current_status == AdvertisementStatus.closed.ToString())
                {
                    // when closed advertisement has no expired date, set the expired date to the current date
                    if (ad.expired_date == null) ad.expired_date = DateTime.Now;

                    if (ad.expired_date <= DateTime.Now)
                    {
                        // there are no any seekers that saved this advertisement
                        if (ad.savedSeekers == null) continue;

                        foreach (var seeker in ad.savedSeekers)
                        {
                            if (seeker.savedAdvertisemnts == null) continue;

                            // remove the advertisement from the seeker's saved advertisements
                            seeker.savedAdvertisemnts.Remove(ad);
                        }
                    }
                }
                else
                {
                    ad.expired_date = null;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExpired(int advertisementId)
        {
            var advertisement = await FindById(advertisementId);

            return IsExpired(advertisement);
        }

        private bool IsExpired(Advertisement advertisement)
        {
            if (advertisement.submission_deadline == null)
            {
                return false;
            }

            return DateTime.Now > advertisement.submission_deadline;
        }

        private bool IsActive(Advertisement advertisement)
        {
            if (advertisement.current_status != AdvertisementStatus.active.ToString())
            {
                return false;
            }

            return true;
        }

        private bool IsSaved(Advertisement advertisement, Seeker seeker)
        {
            if (advertisement.savedSeekers is null || !advertisement.savedSeekers.Contains(seeker))
            {
                return false;
            }

            return true;
        }

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
                throw new InvalidDataException("Invalid status.");
            }
        }

        public async Task<IEnumerable<AdvertisementShortDto>> AdvanceSearch(SearchJobRequestDto requestAdsDto, int seekerID)
        {
            var advertisements = await FindAll(true);

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

            return await CreateAdvertisementList(filteredAdvertisements, seekerID, false);
        }
    }
}
