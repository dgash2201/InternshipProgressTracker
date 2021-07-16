using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Contains statuses of Internship Stream
    /// </summary>
    public enum InternshipStreamStatus
    {
        NotStarted,
        Active,
        Completed
    }

    /// <summary>
    /// Represents the InternshipStream entity
    /// </summary>
    public class InternshipStream : ISoftDeletable
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }

        public ICollection<StudyPlan> StudyPlans { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Mentor> Mentors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
