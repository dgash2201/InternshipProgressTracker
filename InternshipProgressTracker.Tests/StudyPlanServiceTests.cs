using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Utils.Mapper;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
using InternshipProgressTracker.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Tests
{
    /// <summary>
    /// Tests for study plan service
    /// </summary>
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

        /// <summary>
        /// Checks that GetAsync returns study plan successfully by id
        /// </summary>
        [Test]
        public async Task GetAsync_ReturnsSuccessfullyById()
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

        /// <summary>
        /// Checks that GetAsync throws not found exception for unexisted study plan id
        /// </summary>
        [Test]
        public void GetAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.GetAsync(1));
        }

        /// <summary>
        /// Checks that CreateAsync creates study plan successfully
        /// </summary>
        [Test]
        public async Task CreateAsync_CreatesSuccessfully()
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

        /// <summary>
        /// Checks that CreateAsync throws not found exception for unexisted study plan id
        /// </summary>
        [Test]
        public void CreateAsync_ThrowsForUnexistedInternshipStreamId()
        {
            var createDto = new StudyPlanDto
            {
                Title = "Study plan",
                InternshipStreamId = 2,
            };

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.CreateAsync(createDto));
        }

        /// <summary>
        /// Checks that UpdateAsync throws not found exception for unexisted study plan id
        /// </summary>
        [Test]
        public void UpdateAsync_ThrowsForUnexistedId()
        {
            var updateDto = new StudyPlanDto
            {
                Title = "Updated study plan",
            };

            var unexistedId = 1;

            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.UpdateAsync(unexistedId, updateDto));
        }

        /// <summary>
        /// Checks that UpdateAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public async Task UpdateAsync_ThrowsForUnexistedInternshipStreamId()
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

        /// <summary>
        /// Checks that UpdateAsync updates study plan successfully
        /// </summary>
        [Test]
        public async Task UpdateAsync_UpdatesSuccessfully()
        {
            var studyPlanId = 1;

            _dbContext.StudyPlans.Add(new StudyPlan
            {
                Id = studyPlanId,
                InternshipStreamId = ExistedStreamId,
                Title = "Study plan",
            });

            await _dbContext.SaveChangesAsync();

            var newTitle = "Updated study plan";
            var updateDto = new StudyPlanDto
            {
                Title = newTitle,
                InternshipStreamId = ExistedStreamId,
            };

            await _studyPlanService.UpdateAsync(studyPlanId, updateDto);

            var updatedStudyPlan = await _dbContext.StudyPlans.FindAsync(studyPlanId);

            Assert.IsNotNull(updatedStudyPlan);
            Assert.AreEqual(updatedStudyPlan.Title, newTitle);
        }

        /// <summary>
        /// Checks that SoftDeleteAsync throws not found exception for unexisted study plan id
        /// </summary>
        [Test]
        public void SoftDeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.SoftDeleteAsync(1));
        }

        /// <summary>
        /// Checks that DeleteAsync throws not found exception for unexisted study plan id
        /// </summary>
        [Test]
        public void DeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studyPlanService.DeleteAsync(1));
        }
    }
}
