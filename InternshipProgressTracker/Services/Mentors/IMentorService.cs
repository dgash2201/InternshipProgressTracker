using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Models.Mentors;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using InternshipProgressTracker.Models.Users;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Mentors
{
    public interface IMentorService
    {
        Task CreateAsync(User user);
        Task<StudentProgressResponseDto> AddNotesAsync(MentorNotesDto notesDto, CancellationToken cancellationToken = default);
        Task<StudentProgressResponseDto> GradeStudentProgressAsync(GradeProgressDto gradeProgressDto, CancellationToken cancellationToken = default);
        Task<UserResponseDto> SetStudentGradeAsync(int studentId, StudentGrade grade, CancellationToken cancellationToken = default);
    }
}
