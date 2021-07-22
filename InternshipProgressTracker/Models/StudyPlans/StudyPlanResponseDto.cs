using InternshipProgressTracker.Entities;
using System.Collections.Generic;

namespace InternshipProgressTracker.Models.StudyPlans
{
    public class StudyPlanResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int InternshipStreamId { get; set; }

        public IReadOnlyCollection<StudyPlanEntry> Entries { get; set; }
    }
}
