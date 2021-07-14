using System.Collections.Generic;
using System.Threading.Tasks;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Repositories.InternshipStreams
{
    /// <summary>
    /// Internship stream repository interface
    /// </summary>
    public interface IInternshipStreamRepository
    {
        Task<IReadOnlyCollection<InternshipStream>> Get();
        Task<InternshipStream> Get(int id);
        Task<int> Add(InternshipStream internshipStream);
        Task Update(InternshipStream internshipStream);
        Task Delete(int id);
    }
}
