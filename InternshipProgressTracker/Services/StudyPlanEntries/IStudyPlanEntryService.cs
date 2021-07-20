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
        Task<IReadOnlyCollection<StudyPlanEntry>> Get();
        Task<StudyPlanEntry> Get(int id);
        Task<int> Create(CreateStudyPlanEntryDto createDto);
        Task Update(int id, UpdateStudyPlanEntryDto updateDto);
        Task SoftDelete(int id);
        Task Delete(int id);
    }
}
