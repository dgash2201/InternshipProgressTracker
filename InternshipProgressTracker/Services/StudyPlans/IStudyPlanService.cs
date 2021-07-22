using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.StudyPlans;
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
        Task<int> CreateAsync(CreateStudyPlanDto createDto);
        Task UpdateAsync(int id, UpdateStudyPlanDto updateDto);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
