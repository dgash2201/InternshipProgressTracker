using System;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.Students
{
    /// <summary>
    /// Contains data for grading student progress by mentor
    /// </summary>
    public class GradeProgressDto
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        public int GradingMentorId { get; set; }

        [Range(0, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Grade { get; set; }
    }
}
