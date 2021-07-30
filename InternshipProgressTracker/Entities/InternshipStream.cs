using InternshipProgressTracker.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the InternshipStream entity
    /// </summary>
    public class InternshipStream : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public InternshipStreamStatus Status { get; set; }

        public DateTime? PlanStartDate { get; set; }

        public DateTime? FactStartDate { get; set; }

        public DateTime? PlanEndDate { get; set; }

        public DateTime? FactEndDate { get; set; }

        public ICollection<StudyPlan> StudyPlans { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Mentor> Mentors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
