using System.Threading.Tasks;
using InternshipProgressTracker.Models.InternshipStreams;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    public interface IInternshipStreamService
    {
        Task<int> Create(CreateInternshipStreamDto createDto);
        Task Delete(int id);
    }
}
