using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.Students
{
    public interface IStudentRepository
    {
        Task Add(Student student);
    }
}
