using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Mapper;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.InternshipStreams;
using InternshipProgressTracker.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Tests
{
    /// <summary>
    /// Tests for internship stream service
    /// </summary>
    class InternshipStreamServiceTests
    {
        private InternshipProgressTrackerDbContext _dbContext;
        private IInternshipStreamService _internshipStreamService;
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
            _internshipStreamService = new InternshipStreamService(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.DisposeDbContext();
        }

        /// <summary>
        /// Checks that AddStudentAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public async Task InternshipStreamService_AddStudentAsync_ThrowsForUnexistedStudentId()
        {
            var streamId = 1;

            _dbContext.InternshipStreams.Add(new InternshipStream
            {
                Id = streamId
            });

            await _dbContext.SaveChangesAsync();

            var studentId = 1;

            Assert.ThrowsAsync<NotFoundException>(() => _internshipStreamService.AddStudentAsync(streamId, studentId));
        }

        /// <summary>
        /// Checks that AddStudentAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public async Task InternshipStreamService_AddStudentAsync_ThrowsForUnexistedStreamId()
        {
            var studentId = 1;

            _dbContext.Students.Add(new Student
            {
                Id = studentId
            });

            await _dbContext.SaveChangesAsync();

            var streamId = 1;

            Assert.ThrowsAsync<NotFoundException>(() => _internshipStreamService.AddStudentAsync(streamId, studentId));
        }

        /// <summary>
        /// Checks that AddStudentAsync successfully adds student to internship stream
        /// </summary>
        [Test]
        public async Task InternshipStreamService_AddStudentAsync_AddsStudentSuccessfully()
        {
            var streamId = 1;

            _dbContext.InternshipStreams.Add(new InternshipStream
            {
                Id = streamId,
            });

            var studentId = 1;

            _dbContext.Students.Add(new Student
            {
                Id = studentId,
            });

            await _dbContext.SaveChangesAsync();

            await _internshipStreamService.AddStudentAsync(streamId, studentId);

            var internshipStream = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .FirstOrDefaultAsync();

            var student = internshipStream
                .Students
                .FirstOrDefault();

            Assert.NotNull(student);
            Assert.AreEqual(student.Id, studentId);
        }

        /// <summary>
        /// Checks that GetAsync returns internship stream successfully
        /// </summary>
        [Test]
        public async Task InternshipStreamService_GetAsync_ReturnsSuccessfullyById()
        {
            var id = 1;
            var title = "Internship stream";

            _dbContext.InternshipStreams.Add(new Entities.InternshipStream()
            {
                Id = id,
                Title = title,
            });

            await _dbContext.SaveChangesAsync();

            var internshipStreamResponseDto = await _internshipStreamService.GetAsync(id);

            Assert.NotNull(internshipStreamResponseDto);
            Assert.AreEqual(id, internshipStreamResponseDto.Id);
            Assert.AreEqual(title, internshipStreamResponseDto.Title);
        }

        /// <summary>
        /// Checks that GetAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public void InternshipStreamService_GetAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _internshipStreamService.GetAsync(1));
        }

        /// <summary>
        /// Checks that CreateAsync creates internship stream successfully
        /// </summary>
        [Test]
        public async Task InternshipStreamService_CreateAsync_CreatesSuccessfully()
        {
            var title = "Internhsip stream";
            var status = InternshipStreamStatus.NotStarted;

            var createDto = new InternshipStreamDto
            {
                Title = title,
                Status = status,
            };

            await _internshipStreamService.CreateAsync(createDto);

            var responseDto = await _dbContext
                .InternshipStreams
                .FirstOrDefaultAsync();

            Assert.NotNull(responseDto);
            Assert.AreEqual(title, responseDto.Title);
            Assert.AreEqual(status, responseDto.Status);
        }

        /// <summary>
        /// Checks that SoftDeleteAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public void InternshipStreamService_SoftDeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _internshipStreamService.SoftDeleteAsync(1));
        }

        /// <summary>
        /// Checks that DeleteAsync throws not found exception for unexisted internship stream id
        /// </summary>
        [Test]
        public void InternshipStreamService_DeleteAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _internshipStreamService.DeleteAsync(1));
        }
    }
}
