using InternshipProgressTracker.Models.InternshipStreams;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Internship stream service interface
    /// </summary>
    public interface IInternshipStreamService
    {
        Task<InternshipStreamResponseDto> AddMentorAsync(int streamId, int mentorId, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> AddStudentAsync(int streamId, int studentId, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> RemoveMentorAsync(int streamId, int mentorId, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> RemoveStudentAsync(int streamId, int studentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetWithSoftDeletedAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync(int? studentId, int? mentorId, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> CreateAsync(InternshipStreamDto createDto, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> UpdateAsync(int id, InternshipStreamDto updateDto, CancellationToken cancellationToken = default);
        Task<InternshipStreamResponseDto> UpdateAsync(int id, JsonPatchDocument<InternshipStreamDto> patchDocument, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
