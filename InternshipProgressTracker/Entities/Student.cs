using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public int? CurrentGrade { get; set; }

        public User User { get; set; }

        public ICollection<StudentStudyPlanProgress> StudyPlanProgresses { get; set; }
    }
}
