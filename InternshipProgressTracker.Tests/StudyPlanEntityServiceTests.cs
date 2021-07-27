using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Mapper;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlanEntries;
using InternshipProgressTracker.Services.StudyPlans;
using InternshipProgressTracker.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Tests
{
    class StudyPlanEntityServiceTests
    {
        private InternshipProgressTrackerDbContext _dbContext;
        private IStudyPlanEntryService _studyPlanEntryService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
            _dbContext = DbContextInitializer.CreateDbContext();
            _studyPlanEntryService = new StudyPlanEntryService(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.DisposeDbContext();
        }

        [Test]
        public async Task StudyPlanEntryService_GetAsync_ReturnsSuccessfullyById()
        {
            var id = 1;
            var title = "Study plan entry";

            _dbContext.StudyPlanEntries.Add(new StudyPlanEntry
            {
                Id = id,
                Title = title,
            });

            await _dbContext.SaveChangesAsync();

            var responseDto = await _studyPlanEntryService.GetAsync(id);

            Assert.NotNull(responseDto);
            Assert.AreEqual(id, responseDto.Id);
            Assert.AreEqual(title, responseDto.Title);
        }

        [Test]
        public void StudyPlanEntryService_GetAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanEntryService.GetAsync(1));
        }

        [Test]
        public async Task StudyPlanEntryService_CreateAsync_CreatesSuccessfully()
        {
            var studyPlanId = 1;
            _dbContext.StudyPlans.Add(new StudyPlan
            {
                Id = studyPlanId,
            });

            var studyPlanEntryTitle = "Study plan";

            var createDto = new StudyPlanEntryDto
            {
                Title = studyPlanEntryTitle,
                StudyPlanId = studyPlanId,
            };

            await _studyPlanEntryService.CreateAsync(createDto);

            var responseDto = await _dbContext
                .StudyPlanEntries
                .FirstOrDefaultAsync();

            Assert.NotNull(responseDto);
            Assert.AreEqual(studyPlanEntryTitle, responseDto.Title);
        }

        [Test]
        public void StudyPlanEntryService_CreateAsync_ThrowsForUnexistedStudyPlanId()
        {
            var createDto = new StudyPlanEntryDto
            {
                Title = "Study plan entry",
                StudyPlanId = 1,
            };

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanEntryService.CreateAsync(createDto));
        }

        [Test]
        public void StudyPlanEntryService_SoftDeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanEntryService.SoftDeleteAsync(1));
        }

        [Test]
        public void StudyPlanEntryService_DeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanEntryService.DeleteAsync(1));
        }
    }
}
