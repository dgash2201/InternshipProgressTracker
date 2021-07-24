using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Contains roles of a mentor
    /// </summary>
    public enum MentorRole
    {
        Mentor,
        Lead
    }

    /// <summary>
    /// Represents the Mentor entity
    /// </summary>
    public class Mentor : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public MentorRole Role { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public int InternshipStreamId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudentStudyPlanProgresses { get; set; }

        public bool IsDeleted { get; set; }
    }
}
