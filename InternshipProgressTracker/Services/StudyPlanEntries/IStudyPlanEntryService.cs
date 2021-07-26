using InternshipProgressTracker.Models.StudyPlanEntries;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlanEntries
{
    /// <summary>
    /// Interface of study plan entry service
    /// </summary>
    public interface IStudyPlanEntryService
    {
        Task<IReadOnlyCollection<StudyPlanEntryResponseDto>> GetWithSoftDeletedAsync();
        Task<IReadOnlyCollection<StudyPlanEntryResponseDto>> GetAsync();
        Task<StudyPlanEntryResponseDto> GetAsync(int id);
        Task<int> CreateAsync(StudyPlanEntryDto createDto);
        Task UpdateAsync(int id, StudyPlanEntryDto updateDto);
        Task UpdateAsync(int id, JsonPatchDocument<StudyPlanEntryDto> patchDocument);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
