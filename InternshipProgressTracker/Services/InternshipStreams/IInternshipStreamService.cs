using System.Threading.Tasks;
using InternshipProgressTracker.Models.InternshipStreams;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    public interface IInternshipStreamService
    {
        Task<int> Create(CreateInternshipStreamDto createDto);
        Task Update(int id, UpdateInternshipStreamDto updateDto);
        Task Delete(int id);
    }
}
