using System;
using System.ComponentModel.DataAnnotations;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    public class InternshipStreamDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public InternshipStreamStatus Status { get; set; }

        public DateTime? PlanStartDate { get; set; }

        public DateTime? FactStartDate { get; set; }

        public DateTime? PlanEndDate { get; set; }

        public DateTime? FactEndDate { get; set; }
    }
}
