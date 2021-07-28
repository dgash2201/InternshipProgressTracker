using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<IReadOnlyCollection<Student>> GetAsync()
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
        public async Task<Student> GetAsync(int id)
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
        public async Task CreateAsync(User user)
        {
            var student = new Student
            {
                User = user,
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Sets student grade
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public async Task SetStudentGradeAsync(int studentId, StudentGrade grade)
        {
            var student = await _dbContext
                .Students
                .FindAsync(studentId);

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            student.CurrentGrade = grade;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Mark study plan entry as started by student
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="entryId">Id of study plan entry</param>
        public async Task StartStudyPlanEntryAsync(int studentId, int entryId)
        {
            var student = await _dbContext
                .Students
                .FindAsync(studentId);

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entry = await _dbContext
                .StudyPlanEntries
                .FindAsync(entryId);

            if (entry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studentProgress = new StudentStudyPlanProgress
            {
                StudentId = studentId,
                StudyPlanEntryId = entryId,
                Student = student,
                StudyPlanEntry = entry,
                StartTime = DateTime.Now,
            };

            _dbContext.StudentStudyPlanProgresses.Add(studentProgress);
            await _dbContext.SaveChangesAsync();
        }
    }
}
