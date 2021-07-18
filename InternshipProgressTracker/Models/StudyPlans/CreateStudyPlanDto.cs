using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.StudyPlans
{
    public class CreateStudyPlanDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
