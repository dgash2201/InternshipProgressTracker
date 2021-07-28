using InternshipProgressTracker.Entities;
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
        Task SetStudentGradeAsync(int studentId, StudentGrade grade);
    }
}
