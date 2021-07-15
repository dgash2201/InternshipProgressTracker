﻿using InternshipProgressTracker.Entities;
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
            return await _dbContext
                .Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        /// <param name="user">User entity</param>
        public async Task<int> Create(User user)
        {
            var student = new Student
            {
                User = user,
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            return student.Id;
        }

        /// <summary>
        /// Binds student with internship stream
        /// </summary>
        public async Task SetStreamId(Student student, int streamId)
        {
            student.Id = streamId;
            _dbContext.Entry(student).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
