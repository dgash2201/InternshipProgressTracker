using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Mentors;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using InternshipProgressTracker.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Mentors
{
    /// <summary>
    /// Logic service which works with students
    /// </summary>
    public class MentorService : IMentorService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public MentorService(InternshipProgressTrackerDbContext dbContext, 
            IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        public async Task CreateAsync(User user)
        {
            var mentor = new Mentor
            {
                Id = user.Id,
                User = user
            };

            _dbContext.Mentors.Add(mentor);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds mentor notes
        /// </summary>
        public async Task<StudentProgressResponseDto> AddNotesAsync(MentorNotesDto notesDto, CancellationToken cancellationToken = default)
        {
            var studentExists = await _dbContext
                .Students
                .AnyAsync(s => s.Id == notesDto.StudentId, cancellationToken);

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
                .FindAsync(new object[] { notesDto.StudentId, notesDto.StudyPlanEntryId }, cancellationToken);

            if (studentProgress == null || studentProgress.FinishTime == null)
            {
                throw new BadRequestException("Study plan entry was not start by this student");
            }

            studentProgress.MentorNotes = notesDto.Notes;

            _dbContext.StudentStudyPlanProgresses.Update(studentProgress);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudentProgressResponseDto>(studentProgress);
        }

        /// <summary>
        /// Set grade to student progress
        /// </summary>
        public async Task<StudentProgressResponseDto> GradeStudentProgressAsync(
            GradeProgressDto gradeProgressDto, CancellationToken cancellationToken = default)
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

            return _mapper.Map<StudentProgressResponseDto>(studentProgress);
        }

        /// <summary>
        /// Sets student grade
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="grade">Grade of his work</param>
        public async Task<UserResponseDto> SetStudentGradeAsync(int studentId, StudentGrade grade, CancellationToken cancellationToken = default)
        {
            var user = await _userManager
                .Users
                .Include(u => u.Roles)
                .Include(u => u.Student)
                .ThenInclude(s => s.StudyPlanProgresses)
                .Include(u => u.Mentor)
                .ThenInclude(m => m.StudentStudyPlanProgresses)
                .FirstOrDefaultAsync(u => u.Id == studentId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            user.Student.CurrentGrade = grade;

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
