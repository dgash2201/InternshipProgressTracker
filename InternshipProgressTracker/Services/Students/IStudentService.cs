using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Student service interface
    /// </summary>
    public interface IStudentService
    {
        Task CreateAsync(User user, CancellationToken cancellationToken = default);
        Task<StudentProgressResponseDto> AddNotesAsync(int studentId, NotesDto notesDto, CancellationToken cancellationToken = default);
        Task<StudentProgressResponseDto> StartStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default);
        Task<StudentProgressResponseDto> FinishStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default);
    }
}
