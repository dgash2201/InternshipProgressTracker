using InternshipProgressTracker.Database;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Tests
{
    /// <summary>
    /// Tests for student service
    /// </summary>
    public class StudentServiceTests
    {
        private InternshipProgressTrackerDbContext _dbContext;
        private IStudentService _studentService;

        [SetUp]
        public void Setup()
        {
            _dbContext = DbContextInitializer.CreateDbContext();
            _studentService = new StudentService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.DisposeDbContext();
        }

        /// <summary>
        /// Checks get async returns student successfully by id
        /// </summary>
        [Test]
        public async Task StudentService_GetAsync_ReturnsSuccessfully()
        {
            // Arrange
            _dbContext.Students.Add(new Entities.Student() {Id = 1 });
            await _dbContext.SaveChangesAsync();

            // Act
            var dbStudent = await _studentService.GetAsync(1);

            // Assert
            Assert.NotNull(dbStudent);
            Assert.AreEqual(1, dbStudent.Id);
        }

        /// <summary>
        /// Checks GetAsync throws not found exception for unexisted student id
        /// </summary>
        [Test]
        public void StudentService_GetAsync_ThrowsForUnexistedId()
        {
            Assert.ThrowsAsync<NotFoundException>(() => _studentService.GetAsync(1));
        }
    }
}