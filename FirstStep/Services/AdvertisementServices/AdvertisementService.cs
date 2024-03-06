﻿using AutoMapper;
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

        public AdvertisementService(
            DataContext context, 
            IMapper mapper,
            IProfessionKeywordService keywordService,
            ISkillService skillService,
            ISeekerService seekerService)
        {
            _context = context;
            _mapper = mapper;
            _keywordService = keywordService;
            _skillService = skillService;
            _seekerService = seekerService;
        }

        enum AdvertisementStatus { Active, Closed }

        public async Task<IEnumerable<Advertisement>> FindAll()
        {
            // get all active advertisements
            return await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("hrManager")
                .Include("skills")
                .Include("savedSeekers")
                .Where(x => x.current_status == AdvertisementStatus.Active.ToString())
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

            return advertisementDto;
        }

        public async Task<IEnumerable<JobOfferDto>> GetAdvertisementsByCompanyID(int companyID, string status)
        {
            ValidateStatus(status);

            var dbAdvertisements = await FindByCompanyID(companyID);

            // map to jobofferDtos
            var jobOfferDtos = new List<JobOfferDto>();

            foreach (var ad in dbAdvertisements)
            {
                var jobOfferDto = _mapper.Map<JobOfferDto>(ad);

                if (status != "all" && ad.current_status != status)
                {
                    continue;
                }

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

            newAdvertisement.current_status = AdvertisementStatus.Active.ToString();
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
            dbAdvertisement.professionKeywords = await IncludeKeywordsToAdvertisement(reqAdvertisement.keywords, dbAdvertisement.field_id);
            
            // update skills in the advertisement
            dbAdvertisement.skills = await IncludeSkillsToAdvertisement(reqAdvertisement.skills);

            await _context.SaveChangesAsync();
        }

        public async Task SaveAdvertisement(int advertisementId, int seekerId)
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
            
            if (!advertisement.savedSeekers.Contains(seeker))
            {
                // add the seeker to the list of saved seekers
                advertisement.savedSeekers.Add(seeker);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnsaveAdvertisement(int advertisementId, int seekerId)
        {
            // find the seeker
            var seeker = await _seekerService.GetById(seekerId);

            // find the advertisement
            var advertisement = await FindById(advertisementId);

            // check whether the advertisement is already saved
            
            if (advertisement.savedSeekers is null)
            {
                // no any saved seeker for the advertiement
                throw new Exception("Advertisement is not saved by any seeker.");
            }

            if (advertisement.savedSeekers.Contains(seeker))
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

        // map the advertisements to a list of AdvertisementCardDtos and create advertisement list for the seeker
        public async Task<IEnumerable<AdvertisementShortDto>> CreateAdvertisementList(IEnumerable<Advertisement> dbAds, int seekerID)
        {
            var adCardDtos = new List<AdvertisementShortDto>();

            foreach (var ad in dbAds)
            {
                var adDto = _mapper.Map<AdvertisementShortDto>(ad);

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

        // validate status
        private void ValidateStatus(string status)
        {
            var possibleStatuses = new List<string> 
            { 
                AdvertisementStatus.Active.ToString(), 
                AdvertisementStatus.Closed.ToString(), 
                "all" 
            };

            if (!possibleStatuses.Contains(status))
            {
                throw new Exception("Invalid status.");
            }
        }

        // temp function
        // random function to add 10 advertisements to the database
        private async Task AddRandomAdvertisements(int limit)
        {
            var keywordArray = new List<string>() { "C#", "python", "ASP.NET", "MVC", "SQL", "Azure", "Entity Framework", "LINQ", "Web API", "RESTful", "SOAP", "WCF", "WPF", "Xamarin", "Blazor", "Razor", "SignalR", "Angular", "React", "Vue", "Bootstrap", "jQuery", "HTML", "CSS", "JavaScript", "TypeScript", ".NET Core", ".NET Framework", ".NET Standard", "Visual Studio", "Git", "GitHub", "DevOps", "Agile", "Scrum", "TDD", "BDD", "SOLID", "Design Patterns", "OOP", "Microservices", "Docker", "Kubernetes", "AWS", "Machine Learning", "AI", "Data Science", "Python", "R", "TensorFlow" };
            
            var titleArray = new List<string>() { "Web Developer", "Software Engineer", "Data Analyst", "Network Administrator", "Database Administrator", "Cybersecurity Analyst", "Cloud Engineer", "Machine Learning Engineer", "Artificial Intelligence Engineer", "Software Tester", "Technical Support Specialist", "IT Project Manager", "Business Analyst", "UX Designer", "UI Developer", "Game Developer", "Blockchain Developer", "DevOps Engineer", "IT Consultant", "Webmaster" };

            var random = new Random();

            for (int i = 0; i < limit; i++)
            {
                var addAdvertisementDto = new AddAdvertisementDto
                {
                    job_number = random.Next(100, 999),
                    title = titleArray[random.Next(0, titleArray.Count - 1)],
                    country = "Sri Lanka",
                    city = "Colombo",
                    employeement_type = random.Next(0, 1) == 0 ? "Full-time" : "Part-time",
                    arrangement = random.Next(0, 1) == 0 ? "Remote" : "On-site",
                    is_experience_required = random.Next(0, 1) == 1 ? true: false,
                    salary = random.Next(300000, 600000),
                    submission_deadline = DateTime.Now.AddDays(random.Next(25, 45)),
                    job_description = "We are looking for a software developer to join our team. We are a company that values its employees.",
                    hrManager_id = 10, // under bistec
                    field_id = 1, // it and cs
                    keywords = new List<string>(),
                    reqSkills = new List<string>()
                };

                for (int j = 0; j < random.Next(4, 11); j++)
                {
                    addAdvertisementDto.keywords.Add(keywordArray[random.Next(0, keywordArray.Count - 1)].ToLower());
                }

                for (int j = 0; j < random.Next(4, 11); j++)
                {
                    addAdvertisementDto.reqSkills.Add(keywordArray[random.Next(0, keywordArray.Count - 1)].ToLower());
                }

                await Create(addAdvertisementDto);

                Console.Out.WriteLine($"Advertisement added. { i + 1 }");
            }
        }

        public async Task<IEnumerable<AdvertisementShortDto>> BasicSearch(SearchJobRequestDto requestAdsDto, int seekerID)
        {
            if (requestAdsDto == null)
            {
                throw new Exception("Request cannot be null.");
            }
            else
            {
                Console.Out.WriteLine($"Title: {requestAdsDto.title}");
                Console.Out.WriteLine($"Country: {requestAdsDto.country}");
                Console.Out.WriteLine($"City: {requestAdsDto.city}");
                Console.Out.WriteLine($"Arrangement: {requestAdsDto.arrangement}");
                Console.Out.WriteLine($"Employeement type: {requestAdsDto.employeement_type}");
            }

            var advertisements = await _context.Advertisements
                .Include("professionKeywords")
                .Include("job_Field")
                .Include("skills")
                .Include("savedSeekers")
                .Where(ad =>
                    ad.country == requestAdsDto.country &&
                    ad.city == requestAdsDto.city &&
                        (ad.arrangement == requestAdsDto.arrangement ||
                        ad.employeement_type == requestAdsDto.employeement_type) &&
                    ad.current_status == "active")
                .ToListAsync();

            if (requestAdsDto.title == null)
            {
                Console.Out.WriteLine($"Filtered advertisements: {advertisements.Count}");

                return await CreateAdvertisementList(advertisements, seekerID);
            }

            var filteredAdvertisements = new List<Advertisement> { };

            var titles = requestAdsDto.title.Split(' ');

            foreach (var title in titles)
            {
                filteredAdvertisements
                    .AddRange(advertisements
                        .Where(ad => ad.title.ToLower().Contains(title.ToLower()))
                        .ToList());
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

            Console.Out.WriteLine($"Filtered advertisements: {filteredAdvertisements.Count}");  

            return await CreateAdvertisementList(filteredAdvertisements, seekerID);
        }

        public async Task SearchAds()
        {
            //await AddRandomAdvertisements(10);

            var advertisements = await _context.Advertisements.ToListAsync();

            // convert into kdtree
            var tree = new KdTree<float, Advertisement>(3, new FloatMath());

            foreach (var ad in advertisements)
            {
                //tree.Add(new[] { (float)ad.field_id, (float)ad.company_id }, ad);
            }

            // sample search
            var nodes = tree.GetNearestNeighbours(new[] { 30.0f, 20.0f }, 1);

            //var nodes = tree.GetNearestNeighbours(new[] { 30.0f, 20.0f }, 1);

            foreach (var node in nodes)
            {
                Console.Out.WriteLine(node.Value);
            }
        }
    }
}
