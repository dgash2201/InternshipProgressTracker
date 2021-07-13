using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    public interface IStudentService
    {
        Task Create(User user);
    }
}
