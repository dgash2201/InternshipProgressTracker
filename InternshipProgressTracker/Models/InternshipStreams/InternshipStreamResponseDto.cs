using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Models.StudyPlans;
using System;
using System.Collections.Generic;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    public class InternshipStreamResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }

        public DateTime? PlanStartDate { get; set; }

        public DateTime? FactStartDate { get; set; }

        public DateTime? PlanEndDate { get; set; }

        public DateTime? FactEndDate { get; set; }

        public IReadOnlyCollection<StudyPlanResponseDto> StudyPlans { get; set; }

        public IReadOnlyCollection<Student> Students { get; set; }

        public IReadOnlyCollection<Mentor> Mentors { get; set; }
    }
}
