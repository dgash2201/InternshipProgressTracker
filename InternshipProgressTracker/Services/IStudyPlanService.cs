using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.StudyPlans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services
{
    public interface IStudyPlanService
    {
        Task<IReadOnlyCollection<StudyPlan>> Get();
        Task<StudyPlan> Get(int id);
        Task<int> Create(CreateStudyPlanDto createDto);
        Task Update(int id, UpdateStudyPlanDto updateDto);
        Task SoftDelete(int id);
        Task Delete(int id);
    }
}
