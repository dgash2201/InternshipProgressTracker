using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.StudyPlans;
using System.Collections.Generic;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    public class InternshipStreamResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }

        public IReadOnlyCollection<StudyPlanResponseDto> StudyPlans { get; set; }

        public IReadOnlyCollection<Student> Students { get; set; }

        public IReadOnlyCollection<Mentor> Mentors { get; set; }
    }
}
