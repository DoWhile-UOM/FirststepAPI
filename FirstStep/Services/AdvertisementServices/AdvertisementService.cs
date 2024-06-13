using AutoMapper;
using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Helper;
using FirstStep.Validation;
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
        private readonly IFileService _fileService;

        private readonly int AdvertisementExpiredDays = 10;

        public AdvertisementService(
            DataContext context,
            IMapper mapper,
            IProfessionKeywordService keywordService,
            ISkillService skillService,
            ISeekerService seekerService,
            IApplicationService applicationService,
            IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _keywordService = keywordService;
            _skillService = skillService;
            _seekerService = seekerService;
            _applicationService = applicationService;
            _fileService = fileService;
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
                    .Where(x => x.current_status == AdvertisementValidation.Status.active.ToString())
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
                .Include("applications")
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
                .Include("applications")
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
            advertisementDto.company_logo_url = await _fileService.GetBlobUrl(dbAdvertismeent.hrManager!.company!.company_logo!);
            advertisementDto.is_expired = AdvertisementValidation.IsExpired(dbAdvertismeent);

            return advertisementDto;
        }

        public async Task<IEnumerable<AdvertisementShortDto>> GetById(IEnumerable<int> adList, int seekerID)
        {
            var advertisements = new List<Advertisement>();

            foreach (var adId in adList)
            {
                advertisements.Add(await FindById(adId));
            }

            return await CreateSeekerAdvertisementList(advertisements, seekerID, false);
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

            return advertisements.Where(e => e.current_status == AdvertisementValidation.Status.active.ToString()).ToList();
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetCompanyAdvertisementList(int emp_id, string status)
        {
            AdvertisementValidation.CheckStatus(status);

            Employee? employee = await _context.Employees.FindAsync(emp_id);

            if (employee is null)
            {
                throw new NullReferenceException("Employee is not found");
            }

            var dbAdvertisements = await FindByCompanyID(employee.company_id);

            return CreateCompanyAdvertisementList(dbAdvertisements, status, employee);
        }

        public async Task<IEnumerable<AdvertisementTableRowDto>> GetCompanyAdvertisementList(int emp_id, string status, string title)
        {
            AdvertisementValidation.CheckStatus(status);

            Employee? employee = await _context.Employees.FindAsync(emp_id);

            if (employee is null)
            {
                throw new NullReferenceException("Employee is not found");
            }

            if (title != null)
            {
                var dbAdvertisements = await FindByCompanyID(employee.company_id);

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

                return CreateCompanyAdvertisementList(filteredAdvertisements, status, employee);
            }
            else
            {
                return await GetCompanyAdvertisementList(employee.company_id, status);
            }
        }

        public async Task<IEnumerable<AdvertisementHRATableRowDto>> GetAssignedAdvertisementsByHRA(int hra_userID)
        {
            // get hra by user id
            HRAssistant? hra = await _context.HRAssistants
                .Include("applications")
                .Where(hra => hra.user_id == hra_userID)
                .FirstOrDefaultAsync();

            if (hra is null)
            {
                throw new NullReferenceException("HR Assistant not found.");
            }

            if (hra.applications is null)
            {
                return new List<AdvertisementHRATableRowDto>();
            }

            Dictionary<int, Advertisement> assignedAdvertisements = new Dictionary<int, Advertisement>();

            foreach (var application in hra.applications)
            {
                if (!assignedAdvertisements.ContainsKey(application.advertisement_id))
                {
                    // find advertisement by id
                    Advertisement? assignedAd = await FindById(application.advertisement_id);

                    if (assignedAd is not null)
                    {
                        assignedAdvertisements.Add(application.advertisement_id, assignedAd);
                    }
                }
            }

            return CreateHRAAdvertisementList(assignedAdvertisements.Values, hra);
        }

        public async Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, int noOfResultsPerPage)
        {
            var matchingAds = await FindMatchingAdvertisements(seekerID);

            return await CreateFirstPageResults(matchingAds, seekerID, noOfResultsPerPage);
        }

        public async Task<AdvertisementFirstPageDto> GetRecommendedAdvertisements(int seekerID, float longitude, float latitude, int noOfResultsPerPage)
        {
            var matchingAds = await FindMatchingAdvertisements(seekerID, longitude, latitude);

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

            newAdvertisement.current_status = AdvertisementValidation.Status.active.ToString();
            _context.Advertisements.Add(newAdvertisement);
            await _context.SaveChangesAsync();
        }

        public async Task ReactivateAdvertisement(int id, DateTime? submissionDeadline)
        {
            var advertisement = await FindById(id);

            if (AdvertisementValidation.IsActive(advertisement))
            {
                // no need to update the submission deadline, because the advertisement is already active
                return;
            }

            if (submissionDeadline < DateTime.Now)
            {
                throw new InvalidDataException("Submission deadline cannot be in the past.");
            }

            advertisement.current_status = AdvertisementValidation.Status.active.ToString();
            advertisement.submission_deadline = submissionDeadline;
            advertisement.expired_date = null;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatus(int id, string newStatus)
        {
            AdvertisementValidation.CheckStatus(newStatus);
            var advertisement = await FindById(id);

            if (newStatus == AdvertisementValidation.Status.active.ToString() && AdvertisementValidation.IsExpired(advertisement))
            {
                // can't activate an expired advertisement, therefore first need to update the submission deadline
                throw new InvalidDataException("Cannot activate an expired advertisement.");
            }
            else if (newStatus == advertisement.current_status)
            {
                // no need to update the status, because the advertisement is already in the requested status
                return;
            }
            else if (newStatus == AdvertisementValidation.Status.closed.ToString() && AdvertisementValidation.IsActive(advertisement))
            {
                // can't close an active advertisement, therefore first need to update the submission deadline
                throw new BadHttpRequestException("Cannot close an active advertisement.");
            }

            // update the advertisement status
            advertisement.current_status = newStatus;

            if (AdvertisementValidation.IsActive(advertisement))
            {
                advertisement.expired_date = null;
            }
            else
            {
                // set submission deadline to the current date, because need to block application submition anymore
                advertisement.submission_deadline = DateTime.Now;

                // set the expired date to 10 days after the current date, because need to hold saved advertisements for 10 days
                advertisement.expired_date = DateTime.Now.AddDays(AdvertisementExpiredDays);

                try
                {
                    // execute task delegation on expired advertisements
                    await _applicationService.InitiateTaskDelegation(advertisement);
                }
                catch { }
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

            return await CreateSeekerAdvertisementList(advertisements, seekerID, true);
        }

        public async Task<IEnumerable<AppliedAdvertisementShortDto>> GetAppliedAdvertisements(int seekerID)
        {
            Seeker seeker = await _seekerService.GetById(seekerID);

            var appliedAdvertismentList = new List<AppliedAdvertisementShortDto>();

            // find all the applications that send by the seeker
            var submittedApplications = await _applicationService.GetBySeekerId(seeker.user_id);

            // hold the company and and the company logo to increase the performance of searching blob url
            Dictionary<int, (string company_name, string company_logo)> recentAccessedCompanies = new Dictionary<int, (string, string)>();

            int company_id;

            foreach (var submitApplication in submittedApplications)
            {
                var dbAdvertisement = await FindById(submitApplication.advertisement_id);
                var appliedAdvertisement = _mapper.Map<AppliedAdvertisementShortDto>(dbAdvertisement);

                // find the current application status by checking the last revision for the application
                appliedAdvertisement.application_status = _applicationService.GetApplicationStatus(submitApplication);

                appliedAdvertisement.application_id = submitApplication.application_Id;

                company_id = dbAdvertisement.hrManager!.company_id;

                if (!recentAccessedCompanies.ContainsKey(company_id))
                {
                    Company? company = await _context.Companies.FindAsync(company_id);
                    if (company == null) continue;

                    appliedAdvertisement.company_logo_url = await _fileService.GetBlobUrl(company.company_logo!);
                    appliedAdvertisement.company_name = company.company_name;

                    recentAccessedCompanies.Add(company_id, (appliedAdvertisement.company_name, appliedAdvertisement.company_logo_url));
                }
                else
                {
                    appliedAdvertisement.company_name = recentAccessedCompanies[company_id].company_name;
                    appliedAdvertisement.company_logo_url = recentAccessedCompanies[company_id].company_logo;
                }

                appliedAdvertismentList.Add(appliedAdvertisement);
            }

            return appliedAdvertismentList;
        }

        public async Task Delete(int id, bool isConfirmed)
        {
            Advertisement advertisement = await FindById(id);

            if (!isConfirmed)
            {
                // check the advertisement had any applications
                if (advertisement.applications!
                    .Where(a => a.status == ApplicationService.ApplicationStatus.NotEvaluated.ToString())
                    .Count() > 0)
                {
                    throw new InvalidOperationException("Cannot delete an advertisement that has non evaluated applications.");
                }
            }

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
            firstPageResults.FirstPageAdvertisements = await CreateSeekerAdvertisementList(dbAds.Take(noOfresultsPerPage), seekerID, false);

            // add all advertisement ids into a list
            firstPageResults.allAdvertisementIds = dbAds.Select(e => e.advertisement_id).ToList();

            return firstPageResults;
        }

        // map the advertisements to a list of AdvertisementCardDtos and create advertisement list for the seeker
        private async Task<IEnumerable<AdvertisementShortDto>> CreateSeekerAdvertisementList(IEnumerable<Advertisement> dbAds, int seekerID, bool isSaveOnly)
        {
            var adCardDtos = new List<AdvertisementShortDto>();

            Seeker seeker = await _seekerService.GetById(seekerID);

            // hold the company and and the company logo to increase the performance of searching blob url
            Dictionary<int, (string company_name, string company_logo)> recentAccessedCompanies = new Dictionary<int, (string, string)>();

            int company_id;

            foreach (var ad in dbAds)
            {
                var adDto = _mapper.Map<AdvertisementShortDto>(ad);

                // check whether the advertisement is saved by the seeker
                adDto.is_saved = AdvertisementValidation.IsSaved(ad, seeker);
                if (isSaveOnly && !adDto.is_saved) continue;

                // check whether the advertisement is expired or not
                adDto.is_expired = AdvertisementValidation.IsExpired(ad);

                company_id = ad.hrManager!.company_id;

                if (!recentAccessedCompanies.ContainsKey(company_id))
                {
                    Company? company = await _context.Companies.FindAsync(company_id);
                    if (company == null) continue;

                    adDto.company_logo_url = await _fileService.GetBlobUrl(company.company_logo!);
                    adDto.company_name = company.company_name;

                    recentAccessedCompanies.Add(company_id, (adDto.company_name, adDto.company_logo_url));
                }
                else
                {
                    adDto.company_name = recentAccessedCompanies[company_id].company_name;
                    adDto.company_logo_url = recentAccessedCompanies[company_id].company_logo;
                }

                adCardDtos.Add(adDto);
            }

            return adCardDtos;
        }

        // map the advertisements to a list of JobOfferDtos and create advertisement list for the company (Company Admin and HR Manager)
        private IEnumerable<AdvertisementTableRowDto> CreateCompanyAdvertisementList(IEnumerable<Advertisement> dbAds, string status, Employee employee)
        {
            var jobOfferDtos = new List<AdvertisementTableRowDto>();

            foreach (var ad in dbAds)
            {
                if (status != "all" && ad.current_status != status)
                {
                    continue;
                }

                var jobOfferDto = _mapper.Map<AdvertisementTableRowDto>(ad);

                jobOfferDto.field_name = ad.job_Field!.field_name;

                if (employee.user_type != User.UserType.ca.ToString() 
                    && ad.hrManager_id != employee.user_id)
                {
                    jobOfferDto.has_permision_for_handling = false;
                }
                else
                {
                    jobOfferDto.has_permision_for_handling = true;
                }

                jobOfferDto.no_of_applications = ad.applications!.Count();

                jobOfferDto.no_of_evaluated_applications = ad.applications!
                    .Where(a => a.status != ApplicationService.ApplicationStatus.NotEvaluated.ToString()).Count();

                jobOfferDto.no_of_accepted_applications = ad.applications!
                    .Where(a => a.status == ApplicationService.ApplicationStatus.Accepted.ToString()).Count();

                jobOfferDto.no_of_rejected_applications = ad.applications!
                    .Where(a => a.status == ApplicationService.ApplicationStatus.Rejected.ToString()).Count();

                jobOfferDtos.Add(jobOfferDto);
            }

            return jobOfferDtos;
        }

        // map advertisements to a list of AdvertisementHRATableRowDto and create advertisement list for the HR Assistant
        private IEnumerable<AdvertisementHRATableRowDto> CreateHRAAdvertisementList(IEnumerable<Advertisement> dbAds, HRAssistant hra)
        {
            var jobOfferDtos = new List<AdvertisementHRATableRowDto>();

            foreach (var ad in dbAds)
            {
                // select only advertisements that are in hold status
                // because only hold advertisements are suitable for evaluating
                if (ad.current_status != AdvertisementValidation.Status.hold.ToString())
                {
                    continue;
                }

                var jobOfferDto = _mapper.Map<AdvertisementHRATableRowDto>(ad);

                jobOfferDto.field_name = ad.job_Field!.field_name;

                jobOfferDto.no_of_applications = ad.applications!.Count();
                jobOfferDto.no_of_assigned_applications = hra.applications!.Where(a => a.advertisement_id == ad.advertisement_id).Count();
                jobOfferDto.no_of_evaluated_applications = hra.applications!
                    .Where(a => 
                        a.advertisement_id == ad.advertisement_id && 
                        a.status != ApplicationService.ApplicationStatus.NotEvaluated.ToString())
                    .Count();
                jobOfferDto.no_of_nonevaluated_applications = jobOfferDto.no_of_assigned_applications - jobOfferDto.no_of_evaluated_applications;

                jobOfferDtos.Add(jobOfferDto);
            }

            return jobOfferDtos;
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
                    ad.current_status == AdvertisementValidation.Status.active.ToString() &&
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
            var reqCityCoordinate = await Map.GetCoordinates(reqCity.ToLower());

            foreach (Advertisement advertisement in advertisements)
            {
                if (await Map.GetDistance(advertisement.city, reqCityCoordinate) <= reqDistance)
                {
                    filteredAdvertisements.Add(advertisement);
                }
            }

            return filteredAdvertisements;
        }

        private async Task<List<Advertisement>> FindMatchingAdvertisements(int seekerID)
        {
            var seeker = await _seekerService.GetById(seekerID);

            // get all active advertisements in seeker's field
            var advertisements = await FindBySeekerJobFieldID(seekerID);

            if (seeker.skills!.Count <= 0)
            {
                // when seeker has no skills, return all advertisements in the seeker's field
                return advertisements.ToList();
            }

            // hold advertisements that match with the seeker's skills
            Dictionary<Advertisement, float> matchingAdvertisements = FindAdvertisementsMatchingWithSkills(seeker, advertisements);

            return matchingAdvertisements.Keys.ToList();
        }

        private async Task<List<Advertisement>> FindMatchingAdvertisements(int seekerID, float longitude, float latitude)
        {
            var seeker = await _seekerService.GetById(seekerID);

            // get all active advertisements in seeker's field
            var advertisements = await FindBySeekerJobFieldID(seekerID);

            Console.WriteLine(seeker.skills);

            if (seeker.skills!.Count() <= 1)
            {
                // when seeker has no skills, return all advertisements in the seeker's field by lowest distance to highest
                return await FindNearestAdvertisements(advertisements, longitude, latitude);
            }

            // find advertisements matching with the seeker's skills
            Dictionary<Advertisement, float> filteredAds = FindAdvertisementsMatchingWithSkills(seeker, advertisements);

            if (filteredAds.Count() <= 0)
            {
                // when there are no matching advertisements, return all advertisements in the seeker's field by lowest distance to highest
               return await FindNearestAdvertisements(advertisements, longitude, latitude);
            }

            // find the distance between the seeker's city and the advertisement's city
            Dictionary<Advertisement, float> advertisementDistances = await FindDistanceForAdvertisements(longitude, latitude, filteredAds.Keys);

            // round the number of advertisements to the nearest hundred
            if ((int)Math.Round(filteredAds.Count() / 100.0) * 100 > 1000)
            {
                // when are more than 1000 advertisements, filter the advertisements by the mean of the matching skills and distances
                // calculate the mean of the matching skills and distances
                float meanSkills = filteredAds.Values.Sum() / filteredAds.Count;
                float meanDistance = advertisementDistances.Values.Sum() / advertisementDistances.Count;

                // select only advertisements that have greater than the mean of the matching skills
                filteredAds = filteredAds.Where(e => e.Value >= meanSkills).ToDictionary(e => e.Key, e => e.Value);

                // select only advertisements that have less than the mean of the distance
                advertisementDistances = advertisementDistances.Where(e => e.Value <= meanDistance).ToDictionary(e => e.Key, e => e.Value);
            }

            // find the common advertisements between the two dictionaries
            var matchingAdvertisements = new Dictionary<Advertisement, (float skillsMatchingPercentage, float distance)> { };

            foreach (var ad in filteredAds)
            {
                if (!matchingAdvertisements.ContainsKey(ad.Key))
                {
                    matchingAdvertisements.Add(ad.Key, (ad.Value, advertisementDistances[ad.Key]));
                }
            }

            // sort by lowest distance with highest matching skills ratio to highest distance with lowest matching skills ratio
            return SortByNearestNeighbor(matchingAdvertisements);
        }

        private async Task<List<Advertisement>> FindNearestAdvertisements(IEnumerable<Advertisement> advertisements, float longitude, float latitude)
        {
            // find distance between advertisement location to seeker's location
            Dictionary<Advertisement, float> distances = await FindDistanceForAdvertisements(longitude, latitude, advertisements);

            // sort by acoording to distance in acending order
            return SortByDistance(distances);
        }

        private List<Advertisement> SortByNearestNeighbor(Dictionary<Advertisement, (float skillsMatchingPercentage, float distance)> matchingAdvertisements)
        {
            // select the advertisement that has the highest matching skills percentage
            float highestMatchingSkillPercentage = matchingAdvertisements.Values.Max(e => e.skillsMatchingPercentage);

            // select the advertisement that has the lowest distance
            float lowestDistance = matchingAdvertisements.Values.Min(e => e.distance);

            // insert into kd-tree
            var tree = new KdTree<float, Advertisement>(2, new FloatMath());

            foreach (var ad in matchingAdvertisements)
            {
                var key = new[] { ad.Value.skillsMatchingPercentage, ad.Value.distance };
                tree.Add(key, ad.Key);
            }

            int totalResults = matchingAdvertisements.Count;

            if (totalResults > 10)
            {
                // find the nearest neighbors with 80% coverage
                totalResults = (int)(matchingAdvertisements.Count * 0.8);
            }

            var nearestAds = tree.GetNearestNeighbours(new[] { highestMatchingSkillPercentage, lowestDistance }, totalResults);

            return nearestAds.Select(e => e.Value).ToList();
        }

        private List<Advertisement> SortByDistance(Dictionary<Advertisement, float> advertisementDistances)
        {
            // sort by acoording to distance in acending order
            return advertisementDistances.OrderBy(e => e.Value).ToDictionary(e => e.Key, e => e.Value).Keys.ToList();
        }

        private async Task<Dictionary<Advertisement, float>> FindDistanceForAdvertisements(float longitude, float latitude, IEnumerable<Advertisement> advertisements)
        {
            // get distance from the seeker's city to matching advertisements' cities
            Coordinate seekerCityCoordinate = new Coordinate { 
                Latitude = latitude, 
                Longitude = longitude
            };

            // hold advertisements that match with the seeker's skills and distance
            Dictionary<Advertisement, float> advertisementDistances = new Dictionary<Advertisement, float>();

            // recent calculated distances
            Dictionary<string, float> recentCalculatedDistances = new Dictionary<string, float>();

            Coordinate adCityCoordinate;
            float adDistance;

            // calculate the distance between the seeker's city and the advertisement's city
            foreach (var ad in advertisements)
            {
                if (!recentCalculatedDistances.ContainsKey(ad.city.ToLower()))
                {
                    adCityCoordinate = await Map.GetCoordinates(ad.city.ToLower());
                    adDistance = Map.GetDistance(seekerCityCoordinate, adCityCoordinate);

                    recentCalculatedDistances.Add(ad.city.ToLower(), adDistance);
                }
                else
                {
                    adDistance = recentCalculatedDistances[ad.city.ToLower()];
                }

                advertisementDistances.Add(ad, adDistance);
            }

            return advertisementDistances;
        }

        private Dictionary<Advertisement, float> FindAdvertisementsMatchingWithSkills(Seeker seeker, IEnumerable<Advertisement> advertisements)
        {
            Dictionary<Advertisement, float> matchingAdvertisements = new Dictionary<Advertisement, float>();

            // find the seeker's skills
            var seekerSkills = seeker.skills!.Select(e => e.skill_name).ToList();

            // count and add advertisements that match the seeker's skills
            foreach (var ad in advertisements)
            {
                if (ad.skills == null) continue;

                var adSkills = ad.skills.Select(e => e.skill_name).ToList();

                int matchingSkills = 0;

                foreach (var skill in seekerSkills)
                {
                    if (adSkills.Contains(skill))
                    {
                        matchingSkills++;
                    }
                }

                float matchingSkillsPercentage = (float)matchingSkills / ad.skills.Count();

                // select only when matching skills percentage is greater than 50%
                if (matchingSkillsPercentage > 0.5)
                {
                    matchingAdvertisements.Add(ad, matchingSkillsPercentage);
                }
            }

            // sort by a number of matching skills in acending order
            matchingAdvertisements = matchingAdvertisements.OrderByDescending(e => e.Value).ToDictionary(e => e.Key, e => e.Value);

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
                    ad.current_status = AdvertisementValidation.Status.hold.ToString();
                    ad.expired_date = DateTime.Now.AddDays(AdvertisementExpiredDays);

                    try
                    {
                        // execute task delegation on expired advertisements
                        await _applicationService.InitiateTaskDelegation(ad);
                    }
                    catch { continue; }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveSavedExpiredAdvertisements()
        {
            var advertisements = await FindAll(false);

            foreach (var ad in advertisements)
            {
                if (!AdvertisementValidation.IsActive(ad))
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

            return AdvertisementValidation.IsExpired(advertisement);
        }
    }
}
