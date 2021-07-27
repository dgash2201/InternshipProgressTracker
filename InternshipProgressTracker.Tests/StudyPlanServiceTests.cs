using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Mapper;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
using InternshipProgressTracker.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Tests
{
    class StudyPlanServiceTests
    {
        private InternshipProgressTrackerDbContext _dbContext;
        private IStudyPlanService _studyPlanService;
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
            _studyPlanService = new StudyPlanService(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.DisposeDbContext();
        }

        [Test]
        public async Task StudyPlanService_GetAsync_ReturnsSuccessfullyById()
        {
            var id = 1;
            var title = "Study plan";

            _dbContext.StudyPlans.Add(new StudyPlan
            {
                Id = id,
                Title = title,
            });

            await _dbContext.SaveChangesAsync();

            var studyPlanResponseDto = await _studyPlanService.GetAsync(id);

            Assert.NotNull(studyPlanResponseDto);
            Assert.AreEqual(id, studyPlanResponseDto.Id);
            Assert.AreEqual(title, studyPlanResponseDto.Title);
        }

        [Test]
        public void StudyPlanService_GetAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.GetAsync(1));
        }

        [Test]
        public async Task StudyPlanService_CreateAsync_CreatesSuccessfully()
        {
            var internshipStreamId = 1;
            _dbContext.InternshipStreams.Add(new InternshipStream
            {
                Id = internshipStreamId,
            });

            var studyPlanTitle = "Study plan";

            var createDto = new StudyPlanDto
            {
                Title = studyPlanTitle,
                InternshipStreamId = internshipStreamId,
            };

            await _studyPlanService.CreateAsync(createDto);

            var responseDto = await _dbContext
                .StudyPlans
                .FirstOrDefaultAsync();

            Assert.NotNull(responseDto);
            Assert.AreEqual(studyPlanTitle, responseDto.Title);
        }

        [Test]
        public void StudyPlanService_CreateAsync_ThrowsForUnexistedInternshipStreamId()
        {
            var createDto = new StudyPlanDto
            {
                Title = "Study plan",
                InternshipStreamId = 1,
            };

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.CreateAsync(createDto));
        }

        [Test]
        public void StudyPlanService_SoftDeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.SoftDeleteAsync(1));
        }

        [Test]
        public void StudyPlanService_DeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.DeleteAsync(1));
        }
    }
}
