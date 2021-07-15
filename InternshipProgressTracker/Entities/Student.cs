using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the Student entity
    /// </summary>
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public int? InternshipStreamId { get; set; }

        public InternshipStream InternshipStream { get; set; }

        public int? CurrentGrade { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudyPlanProgresses { get; set; }
    }
}
