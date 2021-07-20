﻿using System.ComponentModel.DataAnnotations;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    public class CreateInternshipStreamDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public InternshipStreamStatus Status { get; set; }
    }
}