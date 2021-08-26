using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Logic service which works with students
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentService(InternshipProgressTrackerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        /// <param name="user">User entity</param>
        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            var student = new Student
            {
                Id = user.Id,
                User = user,
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adds student notes
        /// </summary>
        public async Task AddNotesAsync(int studentId, NotesDto notesDto, CancellationToken cancellationToken = default)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == studentId, cancellationToken);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == notesDto.StudyPlanEntryId, cancellationToken);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(new object[] { studentId, notesDto.StudyPlanEntryId }, cancellationToken);

            if (studentProgress == null || studentProgress.FinishTime == null)
            {
                throw new BadRequestException("Study plan entry was not start by this student");
            }

            studentProgress.StudentNotes = notesDto.Notes;

            _dbContext.StudentStudyPlanProgresses.Update(studentProgress);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Set grade to student progress
        /// </summary>
        public async Task GradeStudentProgressAsync(GradeProgressDto gradeProgressDto, CancellationToken cancellationToken = default)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == gradeProgressDto.StudentId, cancellationToken);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == gradeProgressDto.StudyPlanEntryId, cancellationToken);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var mentorExists = await _dbContext
                .Mentors
                .AnyAsync(e => e.Id == gradeProgressDto.GradingMentorId, cancellationToken);

            if (!mentorExists)
            {
                throw new NotFoundException("Mentor with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(new object[] { gradeProgressDto.StudentId, gradeProgressDto.StudyPlanEntryId }, cancellationToken);

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
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Sets student grade
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="grade">Grade of his work</param>
        public async Task SetStudentGradeAsync(int studentId, StudentGrade grade, CancellationToken cancellationToken = default)
        {
            var student = await _dbContext
                .Students
                .FindAsync(new object[] { studentId }, cancellationToken);

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
        public async Task<StudentProgressResponseDto> StartStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default)
        {
            var student = await _dbContext
                .Students
                .FindAsync(new object[] { studentId }, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entry = await _dbContext
                .StudyPlanEntries
                .FindAsync(new object[] { entryId }, cancellationToken);

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
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentProgressResponseDto>(studentProgress);
        }

        /// <summary>
        /// Mark study plan entry as finished by student
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="entryId">Id of study plan entry</param>
        public async Task<StudentProgressResponseDto> FinishStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == studentId, cancellationToken);

            if (!studentExists)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            var entryExists = await _dbContext
                .StudyPlanEntries
                .AnyAsync(e => e.Id == entryId, cancellationToken);

            if (!entryExists)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studentProgress = await _dbContext
                .StudentStudyPlanProgresses
                .FindAsync(new object[] { studentId, entryId }, cancellationToken);

            if (studentProgress == null || studentProgress.StartTime == null)
            {
                throw new BadRequestException("Study plan entry was not started by this student");
            }

            studentProgress.FinishTime = DateTime.Now;

            _dbContext.StudentStudyPlanProgresses.Update(studentProgress);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentProgressResponseDto>(studentProgress);
        }
    }
}
