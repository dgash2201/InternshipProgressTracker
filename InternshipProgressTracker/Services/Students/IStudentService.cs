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
        Task<Student> GetAsync(int id);
        Task<IReadOnlyCollection<Student>> GetAsync();
        Task CreateAsync(User user);
        Task SetStreamAsync(int StudentId, InternshipStream stream);
    }
}
