using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.StudyPlanEntries
{
    public class CreateStudyPlanEntryDto
    {
        public int Id { get; set; }

        public int StudyPlanId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Duration { get; set; }
    }
}
