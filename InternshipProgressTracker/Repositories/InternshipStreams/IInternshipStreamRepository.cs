using System.Threading.Tasks;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Repositories.InternshipStreams
{
    public interface IInternshipStreamRepository
    {
        Task<int> Add(InternshipStream internshipStream);
        Task Delete(int id);
    }
}
