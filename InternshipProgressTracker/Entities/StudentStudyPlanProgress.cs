using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the StudentStudyPlanProgress entity
    /// </summary>
    public class StudentStudyPlanProgress
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        public int GradingMentorId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime FinishTime { get; set; }

        public string StudentNotes { get; set; }

        public string MentorNotes { get; set; }
    }
}
