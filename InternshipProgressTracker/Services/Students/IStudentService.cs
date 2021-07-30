using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Student service interface
    /// </summary>
    public interface IStudentService
    {
        Task<IReadOnlyCollection<Student>> GetAsync();
        Task<Student> GetAsync(int id);
        Task CreateAsync(User user);
        Task AddNotesAsync(int studentId, NotesDto notesDto);
        Task GradeStudentProgressAsync(GradeProgressDto gradeProgressDto);
        Task SetStudentGradeAsync(int studentId, StudentGrade grade);
        Task StartStudyPlanEntryAsync(int studentId, int entryId);
        Task FinishStudyPlanEntryAsync(int studentId, int entryId);
    }
}
