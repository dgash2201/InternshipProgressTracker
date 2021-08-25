using InternshipProgressTracker.Models.StudyPlans;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlans
{
    /// <summary>
    /// Interface of study plan service
    /// </summary>
    public interface IStudyPlanService
    {
        Task<IReadOnlyCollection<StudyPlanResponseDto>> GetWithSoftDeletedAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<StudyPlanResponseDto>> GetAsync(CancellationToken cancellationToken = default);
        Task<StudyPlanResponseDto> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<StudyPlanResponseDto> CreateAsync(StudyPlanDto createDto, CancellationToken cancellationToken = default);
        Task<StudyPlanResponseDto> UpdateAsync(int id, StudyPlanDto updateDto, CancellationToken cancellationToken = default);
        Task<StudyPlanResponseDto> UpdateAsync(int id, JsonPatchDocument<StudyPlanDto> patchDocument, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
