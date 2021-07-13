
using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.Students
{
    public class StudentRepository : IStudentRepository
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;

        public StudentRepository(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Student student)
        {
            _dbContext.Students.Add(student);
           await _dbContext.SaveChangesAsync();
        }
    }
}
