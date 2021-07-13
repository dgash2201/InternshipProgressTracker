using System.Collections.Generic;
using System.Threading.Tasks;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    public interface IInternshipStreamService
    {
        Task<IEnumerable<InternshipStream>> Get();
        Task<InternshipStream> Get(int id);
        Task<int> Create(CreateInternshipStreamDto createDto);
        Task Update(int id, UpdateInternshipStreamDto updateDto);
        Task Delete(int id);
    }
}
