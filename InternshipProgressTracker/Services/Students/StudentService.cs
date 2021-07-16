using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Logic service which works with students
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        
        public StudentService(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the list of students
        /// </summary>
        public async Task<IReadOnlyCollection<Student>> Get()
        {
            var students = await _dbContext
                .Students
                .ToListAsync();
            
            return students.AsReadOnly();
        }

        /// <summary>
        /// Gets a student by id
        /// </summary>
        /// <param name="id">Student id</param>
        public async Task<Student> Get(int id)
        {
            var student =  await _dbContext
                .Students
                .FindAsync(id);

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            return student;
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        /// <param name="user">User entity</param>
        public async Task Create(User user)
        {
            var student = new Student
            {
                User = user,
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Binds student with internship stream
        /// </summary>
        public async Task SetStream(int studentId, InternshipStream stream)
        {
            var student = await Get(studentId);
            student.InternshipStream = stream;
            _dbContext.Entry(student).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
