using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the StudyPlan entity
    /// </summary>
    public class StudyPlan : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int InternshipStreamId { get; set; }

        public InternshipStream InternshipStream { get; set; }

        public ICollection<StudyPlanEntry> Entries { get; set; }

        public bool IsDeleted { get; set; }
    }
}
