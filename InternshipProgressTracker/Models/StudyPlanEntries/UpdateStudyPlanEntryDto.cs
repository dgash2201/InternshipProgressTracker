namespace InternshipProgressTracker.Models.StudyPlanEntries
{
    public class UpdateStudyPlanEntryDto
    {
        public int Id { get; set; }

        public int StudyPlanId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }
    }
}
