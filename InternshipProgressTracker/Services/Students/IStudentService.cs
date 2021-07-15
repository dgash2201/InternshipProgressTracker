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
        Task<Student> Get(int id);
        Task<IReadOnlyCollection<Student>> Get();
        Task Create(User user);
        Task SetStreamId(Student student, int streamId);
    }
}
