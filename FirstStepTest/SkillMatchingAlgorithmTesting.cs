using AutoMapper;
using Azure.Storage.Blobs;
using FirstStep.Data;
using FirstStep.Helper;
using FirstStep.MapperProfile;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace FirstStepTest
{
    public class SkillMatchingAlgorithmTesting: IDisposable
    {
        private readonly DataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IMapper _mapper;

        private readonly ITestOutputHelper _output;

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

        public SkillMatchingAlgorithmTesting(ITestOutputHelper output)
        {
            // Set up DbContext options to use in-memory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_SkillsMatching")
                .Options;

            // setup BlobServiceClient
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
            _fileService = new FileService(_blobServiceClient);
            _seekerService = new SeekerService(_context, _mapper, _skillService, _fileService);
            _employeeService = new EmployeeService(_context, _mapper);
            _applicationService = new ApplicationService(_context, _mapper, _revisionService, _fileService, _employeeService);
            _advertisementService = new AdvertisementService(_context, _mapper, _keywordService, _skillService, _seekerService, _applicationService, _fileService);
            _output = output;
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
        public async Task TestExecutionSpeed_SkillMatchingAlgorithm_Kandy()
        {
            // arange
            int seekerId = 4153;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.636696, Latitude = 7.291418 };

            // Act
            var watch = System.Diagnostics.Stopwatch.StartNew();

            await Test_SkillMatchingAlgorithm(seekerId, seekerLocation);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            // output the execution speed
            _output.WriteLine("Execution speed: " + elapsedMs + " ms");

            Assert.True(true);
        }

        [Fact]
        public async Task TestExecutionSpeed_SkillMatchingAlgorithm_Galle()
        {
            // arange
            int seekerId = 4153;
            Coordinate seekerLocation = new Coordinate { Longitude = 80.220978, Latitude = 6.053519 };

            // Act
            var watch = System.Diagnostics.Stopwatch.StartNew();

            await Test_SkillMatchingAlgorithm(seekerId, seekerLocation);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            // output the execution speed
            _output.WriteLine("Execution speed: " + elapsedMs + " ms");

            Assert.True(true);
        }

        [Fact]
        public async Task TestExecutionSpeed_SkillMatchingAlgorithm_Colombo()
        {
            // arange
            int seekerId = 4153;
            Coordinate seekerLocation = new Coordinate { Longitude = 79.861244, Latitude = 6.927079 };

            // Act
            var watch = System.Diagnostics.Stopwatch.StartNew();

            await Test_SkillMatchingAlgorithm(seekerId, seekerLocation);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            // output the execution speed
            _output.WriteLine("Execution speed: " + elapsedMs + " ms");

            Assert.True(true);
        }

        private async Task Test_SkillMatchingAlgorithm(int seekerId, Coordinate seekerLocation)
        {
            // Act
            AdvertisementFirstPageDto result = await _advertisementService
                .GetRecommendedAdvertisements(seekerId, (float)seekerLocation.Longitude, (float)seekerLocation.Latitude, 100);

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

            Assert.True(result.FirstPageAdvertisements.Count() > 0);
        }

        public void Dispose()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
