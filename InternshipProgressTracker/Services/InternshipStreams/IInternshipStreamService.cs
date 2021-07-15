using System.Collections.Generic;
using System.Threading.Tasks;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Internship stream service interface
    /// </summary>
    public interface IInternshipStreamService
    {
        Task AddStudent(int streamId, int studentId);
        Task<IReadOnlyCollection<InternshipStream>> Get();
        Task<InternshipStream> Get(int id);
        Task<int> Create(CreateInternshipStreamDto createDto);
        Task Update(int id, UpdateInternshipStreamDto updateDto);
        Task Delete(int id);
    }
}
