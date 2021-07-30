using System;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the StudentStudyPlanProgress entity
    /// </summary>
    public class StudentStudyPlanProgress : ISoftDeletable
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        public Student Student { get; set; }

        public StudyPlanEntry StudyPlanEntry { get; set; }

        public int? Grade { get; set; }

        public int? GradingMentorId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public string StudentNotes { get; set; }

        public string MentorNotes { get; set; }

        public bool IsDeleted { get; set; }
    }
}
