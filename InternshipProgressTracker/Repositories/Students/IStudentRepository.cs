using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.Students
{
    /// <summary>
    /// Student repository 
    /// </summary>
    public interface IStudentRepository
    {
        Task<Student> Get(int id);
        Task Add(Student student);
        Task Update(Student student);
    }
}
