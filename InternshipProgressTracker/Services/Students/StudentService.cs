using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Students;
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
                Id = user.Id,
                User = user,
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds student notes
        /// </summary>
        public async Task AddNotesAsync(int studentId, NotesDto notesDto)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == studentId);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == notesDto.StudyPlanEntryId);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(studentId, notesDto.StudyPlanEntryId);

            if (studentProgress == null || studentProgress.FinishTime == null)
            {
                throw new BadRequestException("Study plan entry was not start by this student");
            }

            studentProgress.StudentNotes = notesDto.Notes;

            _dbContext.StudentStudyPlanProgresses.Update(studentProgress);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Set grade to student progress
        /// </summary>
        public async Task GradeStudentProgressAsync(GradeProgressDto gradeProgressDto)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == gradeProgressDto.StudentId);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == gradeProgressDto.StudyPlanEntryId);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var mentorExists = await _dbContext
                .Mentors
                .AnyAsync(e => e.Id == gradeProgressDto.GradingMentorId);

            if (!mentorExists)
            {
                throw new NotFoundException("Mentor with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(gradeProgressDto.StudentId, gradeProgressDto.StudyPlanEntryId);

            if (studentProgress == null || studentProgress.FinishTime == null)
            {
                throw new BadRequestException("Study plan entry was not finished by this student");
            }

            if (studentProgress.Grade != null)
            {
                throw new AlreadyExistsException("Grade already exists");
            }

            studentProgress.Grade = gradeProgressDto.Grade;
            studentProgress.GradingMentorId = gradeProgressDto.GradingMentorId;

            _dbContext.Update(studentProgress);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Sets student grade
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="grade">Grade of his work</param>
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

        /// <summary>
        /// Mark study plan entry as finished by student
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="entryId">Id of study plan entry</param>
        public async Task FinishStudyPlanEntryAsync(int studentId, int entryId)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == studentId);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == entryId);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(studentId, entryId);

            if (studentProgress == null || studentProgress.StartTime == null)
            {
                throw new BadRequestException("Study plan entry was not started by this student");
            }

            studentProgress.FinishTime = DateTime.Now;

            _dbContext.StudentStudyPlanProgresses.Update(studentProgress);
            await _dbContext.SaveChangesAsync();
        }
    }
}
