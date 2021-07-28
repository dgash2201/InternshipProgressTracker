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
        private const int ExistedStreamId = 1;
        private InternshipProgressTrackerDbContext _dbContext;
        private IStudyPlanService _studyPlanService;
        private IMapper _mapper;

        [SetUp]
        public async Task Setup()
        {           
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
            _dbContext = DbContextInitializer.CreateDbContext(); 
            _studyPlanService = new StudyPlanService(_dbContext, _mapper);

            _dbContext.InternshipStreams.Add(new InternshipStream
            {
                Id = ExistedStreamId,
            });

            await _dbContext.SaveChangesAsync();
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
            var studyPlanTitle = "Study plan";

            var createDto = new StudyPlanDto
            {
                Title = studyPlanTitle,
                InternshipStreamId = ExistedStreamId,
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
                InternshipStreamId = 2,
            };

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.CreateAsync(createDto));
        }

        [Test] 
        public void StudyPlanService_UpdateAsync_ThrowsForUnexistedId()
        {
            var updateDto = new StudyPlanDto
            {
                Title = "Updated study plan",
            };

            var unexistedId = 1;

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.UpdateAsync(unexistedId, updateDto));
        }

        [Test]
        public async Task StudyPlanService_UpdateAsync_ThrowsForUnexistedInternshipStreamId()
        {
            var studyPlanId = 1;

            _dbContext.StudyPlans.Add(new StudyPlan
            {
                Id = studyPlanId,
                Title = "Study plan",
            });

            await _dbContext.SaveChangesAsync();

            var updateDto = new StudyPlanDto
            {
                Title = "Updated study plan",
                InternshipStreamId = 2,
            };

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.UpdateAsync(studyPlanId, updateDto));
        }

        [Test]
        public async Task StudyPlanService_UpdateAsync_UpdatesSuccessfully()
        {
            var studyPlanId = 1;

            _dbContext.StudyPlans.Add(new StudyPlan
            {
                Id = studyPlanId,
                Title = "Study plan",
            });

            await _dbContext.SaveChangesAsync();

            var newTitle = "Updated study plan";
            var updateDto = new StudyPlanDto
            {
                Title = newTitle,
            };

            await _studyPlanService.UpdateAsync(studyPlanId, updateDto);

            var updatedStudyPlan = await _dbContext.StudyPlans.FindAsync(studyPlanId);

            Assert.IsNotNull(updatedStudyPlan);
            Assert.AreEqual(updatedStudyPlan.Title, newTitle);
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
