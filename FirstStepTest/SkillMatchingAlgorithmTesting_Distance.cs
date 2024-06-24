using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using FirstStep.Data;
using FirstStep.Helper;
using FirstStep.MapperProfile;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.EntityFrameworkCore;

namespace FirstStepTest
{
    public class SkillMatchingAlgorithmTesting_Distance : IDisposable
    {
        private readonly DataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly StorageSharedKeyCredential _storageSharedKeyCredential;
        private readonly IMapper _mapper;

        private readonly AdvertisementService _advertisementService;
        private readonly SkillService _skillService;
        private readonly SeekerService _seekerService;
        private readonly ApplicationService _applicationService;
        private readonly ProfessionKeywordService _keywordService;
        private readonly RevisionService _revisionService;
        private readonly FileService _fileService;
        //private readonly UserService _userService;
        //private readonly CompanyService _companyService;
        private readonly EmployeeService _employeeService;
        //private readonly EmailService _emailService;

        public SkillMatchingAlgorithmTesting_Distance()
        {
            // Set up DbContext options to use in-memory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_DistanceMatching")
                .Options;

            _storageSharedKeyCredential = new StorageSharedKeyCredential("firststep", "uufTzzJ+uB7BRnKG9cN2RUi0mw92n5lTl2EMvnOTw6xv7sfPQSWBqJxHll+Zn2FNc06cGf8Qgrkb+ASteH1KEQ==");
            _blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=firststep;AccountKey=uufTzzJ+uB7BRnKG9cN2RUi0mw92n5lTl2EMvnOTw6xv7sfPQSWBqJxHll+Zn2FNc06cGf8Qgrkb+ASteH1KEQ==;EndpointSuffix=core.windows.net");

            // Initialize DbContext
            _context = new DataContext(options);

            // Seed the in-memory database with data from the original database
            SeedDatabaseFromOriginal();

            // Initialize AutoMapper using the original profiles
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>(); // Assuming MappingProfile is your AutoMapper profile
            });
            _mapper = config.CreateMapper();

            // Initialize the service with actual dependencies
            _skillService = new SkillService(_context);
            _keywordService = new ProfessionKeywordService(_context, _mapper);
            _revisionService = new RevisionService(_context);
            _fileService = new FileService(_blobServiceClient, _storageSharedKeyCredential);
            _seekerService = new SeekerService(_context, _mapper, _skillService, _fileService);
            _employeeService = new EmployeeService(_context, _mapper);
            _applicationService = new ApplicationService(_context, _mapper, _revisionService, _fileService, _employeeService);
            _advertisementService = new AdvertisementService(_context, _mapper, _keywordService, _skillService, _seekerService, _applicationService, _fileService);
        }

        private void SeedDatabaseFromOriginal()
        {
            //var originalOptions = new DbContextOptionsBuilder<DataContext>()
            //    .UseSqlServer("Data Source=192.248.11.34;Database=jobsearch-app;User ID=JobAppMasterUser;Password=FirstStep2024User;TrustServerCertificate=true")
            //    .Options;

            var originalOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer("Server=tcp:firststepserver.database.windows.net;Initial Catalog=firststepdb;Persist Security Info=False;User ID=adminteam;Password=58ashates88$8;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;")
                .Options;

            using (var originalContext = new DataContext(originalOptions))
            {
                var advertisements = originalContext.Advertisements.AsNoTracking().ToList();
                _context.Advertisements.AddRange(advertisements);

                // Repeat for other entities if needed
                var hrManagers = originalContext.HRManagers.AsNoTracking().ToList();
                _context.HRManagers.AddRange(hrManagers);

                var jobFields = originalContext.JobFields.AsNoTracking().ToList();
                _context.JobFields.AddRange(jobFields);

                var seekers = originalContext.Seekers.AsNoTracking().ToList();
                _context.Seekers.AddRange(seekers);

                var skills = originalContext.Skills.AsNoTracking().ToList();
                _context.Skills.AddRange(skills);

                var applications = originalContext.Applications.AsNoTracking().ToList();
                _context.Applications.AddRange(applications);

                var professionKeywords = originalContext.ProfessionKeywords.AsNoTracking().ToList();
                _context.ProfessionKeywords.AddRange(professionKeywords);

                var revisions = originalContext.Revisions.AsNoTracking().ToList();
                _context.Revisions.AddRange(revisions);

                var companies = originalContext.Companies.AsNoTracking().ToList();
                _context.Companies.AddRange(companies);

                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task TestSkillMatchingAlgorithm_Colombo()
        {
            // arange
            int seekerId = 4;
            Coordinate seekerLocation = new Coordinate { Longitude = 79.861244, Latitude = 6.927079 };

            // Act
            await Test_SkillMatchingAlgorithm_ForDistance(seekerId, seekerLocation);
        }

        [Fact]
        public async Task TestSkillMatchingAlgorithm_Kandy()
        {
            // arange
            int seekerId = 4;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.636696, Latitude = 7.291418 };

            // Act
            await Test_SkillMatchingAlgorithm_ForDistance(seekerId, seekerLocation);
        }

        [Fact]
        public async Task TestSkillMatchingAlgorithm_Galle()
        {
            // arange
            int seekerId = 4;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.220978, Latitude = 6.053519 };

            // Act
            await Test_SkillMatchingAlgorithm_ForDistance(seekerId, seekerLocation);
        }

        [Fact]
        public async Task TestSkillMatchingAlgorithm_Gampaha()
        {
            // arange
            int seekerId = 4;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.014366, Latitude = 7.087310 };

            // Act
            await Test_SkillMatchingAlgorithm_ForDistance(seekerId, seekerLocation);
        }

        [Fact]
        public async Task TestSkillMatchingAlgorithm_Jaffna()
        {
            // arange
            int seekerId = 4;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.005974, Latitude = 9.661623 };

            // Act
            await Test_SkillMatchingAlgorithm_ForDistance(seekerId, seekerLocation);
        }

        private async Task Test_SkillMatchingAlgorithm_ForDistance(int seekerId, Coordinate seekerLocation)
        {
            // find the seekers
            var seekers = await _seekerService.GetAll();

            Assert.True(seekers.Count() > 0);

            // remove all the skills of the seeker
            var seeker = await _seekerService.GetById(seekerId);

            if (seeker.skills!.Count() > 0)
            {
                seeker.skills!.Clear();
                _context.SaveChanges();
            }

            // Act
            AdvertisementFirstPageDto result = await _advertisementService
                .GetRecommendedAdvertisements(seekerId, (float)seekerLocation.Longitude, (float)seekerLocation.Latitude, 100);

            Assert.True(result.FirstPageAdvertisements.Count() > 0);

            // distance array
            List<float> distances = new List<float>();

            // Assert
            // check the distance of each advertisement
            foreach (var advertisement in result.FirstPageAdvertisements)
            {
                float distance = await Map.GetDistance(advertisement.city, seekerLocation);

                // add the distance to the list
                distances.Add(distance);
            }

            // check if the distances are in ascending order
            for (int i = 0; i < distances.Count - 1; i++)
            {
                Assert.True(distances[i] <= distances[i + 1]);
            }
        }

        public void Dispose()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
