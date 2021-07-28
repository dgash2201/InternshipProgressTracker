using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    public enum StudentGrade
    {
        Junior,
        Middle,
        Senior
    }


    /// <summary>
    /// Represents the Student entity
    /// </summary>
    public class Student : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        public StudentGrade? CurrentGrade { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudyPlanProgresses { get; set; }

        public ICollection<InternshipStream> InternshipStreams { get; set; }

        public bool IsDeleted { get; set; }
    }
}
