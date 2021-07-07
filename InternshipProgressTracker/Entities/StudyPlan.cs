using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the StudyPlan entity
    /// </summary>
    public class StudyPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int InternshipStreamId { get; set; }

        public ICollection<StudyPlanEntry> Entries { get; set; }
    }
}
