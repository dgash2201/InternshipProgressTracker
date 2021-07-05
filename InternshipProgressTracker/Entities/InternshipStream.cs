using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public enum InternshipStreamStatus
    {
        Current,
        Finished
    }

    public class InternshipStream
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }

        public ICollection<StudyPlan> StudyPlans { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Mentor> Mentors { get; set; }
    }
}
