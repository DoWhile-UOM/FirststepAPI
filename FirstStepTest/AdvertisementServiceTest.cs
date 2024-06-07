using AutoMapper;
using FirstStep.Data;
using FirstStep.Helper;
using FirstStep.MapperProfile;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstStepTest
{
    public class AdvertisementServiceTest :IDisposable
    {
        private readonly DataContext _context;
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

        public AdvertisementServiceTest()
        {
            // Set up DbContext options to use in-memory database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize DbContext
            _context = new DataContext(options);

            // Seed the in-memory database with data from the original database
            SeedDatabaseFromOriginal();

            // Initialize AutoMapper using the original profiles
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapperProfile>(); // Assuming MappingProfile is your AutoMapper profile
            });
            _mapper = config.CreateMapper();

            // Initialize the service with actual dependencies
            _skillService = new SkillService(_context);
            _seekerService = new SeekerService(_context, _mapper, _skillService);
            _keywordService = new ProfessionKeywordService(_context, _mapper);
            _revisionService = new RevisionService(_context);
            _fileService = new FileService();
            _employeeService = new EmployeeService(_context, _mapper);
            _applicationService = new ApplicationService(_context, _mapper, _revisionService, _fileService, _employeeService);
            _advertisementService = new AdvertisementService(_context, _mapper, _keywordService, _skillService, _seekerService, _applicationService, _fileService);
        }

        private void SeedDatabaseFromOriginal()
        {
            var originalOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer("Data Source=192.248.11.34;Database=jobsearch-app;User ID=JobAppMasterUser;Password=FirstStep2024User;TrustServerCertificate=true")
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

                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task IsExpired_ReturnsTrue_WhenAdvertisementIsExpired()
        {
            // Act
            var result = await _advertisementService.IsExpired(1057);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsExpired_ReturnsFalse_WhenAdvertisementIsNotExpired()
        {
            // Act
            var result = await _advertisementService.IsExpired(1055);

            // Assert
            Assert.False(result);
        }

        public void Dispose()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
