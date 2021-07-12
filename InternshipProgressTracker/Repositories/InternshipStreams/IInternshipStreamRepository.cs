using System.Threading.Tasks;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Repositories.InternshipStreams
{
    public interface IInternshipStreamRepository
    {
        Task<InternshipStream> Get(int id);
        Task<int> Add(InternshipStream internshipStream);
        Task Update(InternshipStream internshipStream);
        Task Delete(int id);
    }
}
