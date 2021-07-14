using InternshipProgressTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.Students
{
    /// <summary>
    /// Works with database
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;

        public StudentRepository(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets student by id
        /// </summary>
        /// <param name="id">Student id</param>
        public async Task<Student> Get(int id)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Adds created student to the database
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task Add(Student student)
        {
            _dbContext.Students.Add(student);
           await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Saves updated student entity to the database
        /// </summary>
        public async Task Update(Student student)
        {
            _dbContext.Entry<Student>(student).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
