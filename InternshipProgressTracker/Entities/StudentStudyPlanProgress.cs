using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public class StudentStudyPlanProgress
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        public int Grade { get; set; }

        public int GradingMentorId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public string StudentNotes { get; set; }

        public string MentorNotes { get; set; }
    }
}
