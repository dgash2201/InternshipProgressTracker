using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    public interface IStudentService
    {
        Task<Student> Get(int id);
        Task Create(User user);
        Task SetStreamId(Student student, int streamId);
    }
}
