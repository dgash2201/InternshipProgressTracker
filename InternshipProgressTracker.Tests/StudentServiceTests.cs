using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Tests.Helpers;
using InternshipProgressTracker.Utils.Mapper;
using NUnit.Framework;

namespace InternshipProgressTracker.Tests
{
    /// <summary>
    /// Tests for student service
    /// </summary>
    public class StudentServiceTests
    {
        private InternshipProgressTrackerDbContext _dbContext;
        private IStudentService _studentService;
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
            _studentService = new StudentService(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.DisposeDbContext();
        }
    }
}