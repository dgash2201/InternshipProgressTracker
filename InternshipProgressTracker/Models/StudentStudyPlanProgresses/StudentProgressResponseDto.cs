using System;

namespace InternshipProgressTracker.Models.StudentStudyPlanProgresses
{
    /// <summary>
    /// Contains student's progress data on study plan entry
    /// </summary>
    public class StudentProgressResponseDto
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        public int? Grade { get; set; }

        public int? GradingMentorId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public string StudentNotes { get; set; }

        public string MentorNotes { get; set; }
    }
}
