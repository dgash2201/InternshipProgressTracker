using InternshipProgressTracker.Models.InternshipStreams;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Internship stream service interface
    /// </summary>
    public interface IInternshipStreamService
    {
        Task AddStudentAsync(int streamId, int studentId);
        Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetWithSoftDeletedAsync();
        Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync(int? studentId, int? mentorId);
        Task<InternshipStreamResponseDto> GetAsync(int id);
        Task<int> CreateAsync(InternshipStreamDto createDto);
        Task UpdateAsync(int id, InternshipStreamDto updateDto);
        Task UpdateAsync(int id, JsonPatchDocument<InternshipStreamDto> patchDocument);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
