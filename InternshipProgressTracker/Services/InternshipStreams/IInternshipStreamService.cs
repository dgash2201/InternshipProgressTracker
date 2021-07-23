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
        Task AddStudentAsync(int streamId, int studentId);
        Task<IReadOnlyCollection<InternshipStream>> GetAsync();
        Task<InternshipStream> GetAsync(int id);
        Task<int> CreateAsync(CreateInternshipStreamDto createDto);
        Task UpdateAsync(int id, UpdateInternshipStreamDto updateDto);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
