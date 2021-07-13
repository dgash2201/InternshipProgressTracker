using InternshipProgressTracker.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Student> Get(int id)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Add(Student student)
        {
            _dbContext.Students.Add(student);
           await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Student student)
        {
            _dbContext.Entry<Student>(student).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
