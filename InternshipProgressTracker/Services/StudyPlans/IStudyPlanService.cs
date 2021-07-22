using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.StudyPlans;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlans
{
    /// <summary>
    /// Interface of study plan service
    /// </summary>
    public interface IStudyPlanService
    {
        Task<IReadOnlyCollection<StudyPlan>> GetAsync();
        Task<StudyPlan> GetAsync(int id);
        Task<int> CreateAsync(StudyPlanDto createDto);
        Task UpdateAsync(int id, StudyPlanDto updateDto);
        Task UpdateAsync(int id, JsonPatchDocument<StudyPlanDto> patchDocument);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
