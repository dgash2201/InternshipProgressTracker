﻿using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Models.StudyPlans
{
    /// <summary>
    /// Contains data for creating a study plan
    /// </summary>
    public class CreateStudyPlanDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
