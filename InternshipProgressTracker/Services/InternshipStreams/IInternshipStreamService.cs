using System.Collections.Generic;
using System.Threading.Tasks;
using InternshipProgressTracker.Models.InternshipStreams;
using Microsoft.AspNetCore.JsonPatch;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Internship stream service interface
    /// </summary>
    public interface IInternshipStreamService
    {
        Task AddStudentAsync(int streamId, int studentId);
        Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync();
        Task<InternshipStreamResponseDto> GetAsync(int id);
        Task<int> CreateAsync(InternshipStreamDto createDto);
        Task UpdateAsync(int id, InternshipStreamResponseDto updateDto);
        Task UpdateAsync(int id, JsonPatchDocument<InternshipStreamDto> patchDocument);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
