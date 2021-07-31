using InternshipProgressTracker.Models.StudyPlanEntries;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlanEntries
{
    /// <summary>
    /// Interface of study plan entry service
    /// </summary>
    public interface IStudyPlanEntryService
    {
        Task<IReadOnlyCollection<StudyPlanEntryResponseDto>> GetWithSoftDeletedAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<StudyPlanEntryResponseDto>> GetAsync(CancellationToken cancellationToken = default);
        Task<StudyPlanEntryResponseDto> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(StudyPlanEntryDto createDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, StudyPlanEntryDto updateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, JsonPatchDocument<StudyPlanEntryDto> patchDocument, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
