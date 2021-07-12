using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    public class UpdateInternshipStreamDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }
    }
}
