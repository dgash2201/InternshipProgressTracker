using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Models.Students;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Student service interface
    /// </summary>
    public interface IStudentService
    {
        Task<IReadOnlyCollection<Student>> GetAsync(CancellationToken cancellationToken = default);
        Task<Student> GetAsync(int id, CancellationToken cancellationToken = default);
        Task CreateAsync(User user, CancellationToken cancellationToken = default);
        Task AddNotesAsync(int studentId, NotesDto notesDto, CancellationToken cancellationToken = default);
        Task GradeStudentProgressAsync(GradeProgressDto gradeProgressDto, CancellationToken cancellationToken = default);
        Task SetStudentGradeAsync(int studentId, StudentGrade grade, CancellationToken cancellationToken = default);
        Task StartStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default);
        Task FinishStudyPlanEntryAsync(int studentId, int entryId, CancellationToken cancellationToken = default);
    }
}
