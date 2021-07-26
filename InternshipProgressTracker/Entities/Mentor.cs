using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the Mentor entity
    /// </summary>
    public class Mentor : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudentStudyPlanProgresses { get; set; }
        
        public ICollection<InternshipStream> InternshipStreams { get; set; }

        public bool IsDeleted { get; set; }
    }
}
