using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.StudyPlans
{
    /// <summary>
    /// Contains data for creating a study plan
    /// </summary>
    public class StudyPlanDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int InternshipStreamId { get; set; }
    }
}
