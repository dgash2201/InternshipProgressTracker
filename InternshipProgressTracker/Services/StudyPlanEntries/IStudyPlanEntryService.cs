using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.StudyPlanEntries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlanEntries
{
    /// <summary>
    /// Interface of study plan entry service
    /// </summary>
    public interface IStudyPlanEntryService
    {
        Task<IReadOnlyCollection<StudyPlanEntry>> GetAsync();
        Task<StudyPlanEntry> GetAsync(int id);
        Task<int> CreateAsync(CreateStudyPlanEntryDto createDto);
        Task UpdateAsync(int id, UpdateStudyPlanEntryDto updateDto);
        Task SoftDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
