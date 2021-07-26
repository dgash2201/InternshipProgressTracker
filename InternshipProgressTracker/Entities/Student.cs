using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the Student entity
    /// </summary>
    public class Student : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        public int? CurrentGrade { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudyPlanProgresses { get; set; }

        public ICollection<InternshipStream> InternshipStreams { get; set; }

        public bool IsDeleted { get; set; }
    }
}
