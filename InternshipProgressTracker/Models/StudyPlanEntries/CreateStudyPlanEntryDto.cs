using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.StudyPlanEntries
{
    public class CreateStudyPlanEntryDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Duration { get; set; }
        
        [Required]
        public int StudyPlanId { get; set; }
    }
}
