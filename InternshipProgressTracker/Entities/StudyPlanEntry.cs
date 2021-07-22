using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the StudyPlanEntry entity
    /// </summary>
    public class StudyPlanEntry : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudyPlanId { get; set; }

        public StudyPlan StudyPlan { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Duration { get; set; }

        public ICollection<StudentStudyPlanProgress> StudentsProgresses { get; set; }

        public bool IsDeleted { get; set; }
    }
}
