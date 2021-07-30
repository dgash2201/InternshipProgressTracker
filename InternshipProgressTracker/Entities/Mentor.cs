using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the Mentor entity
    /// </summary>
    public class Mentor : ISoftDeletable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudentStudyPlanProgresses { get; set; }
        
        public ICollection<InternshipStream> InternshipStreams { get; set; }

        public bool IsDeleted { get; set; }
    }
}
