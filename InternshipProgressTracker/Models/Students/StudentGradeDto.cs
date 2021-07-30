using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Students
{
    public class StudentGradeDto
    {
        public int StudentId { get; set; }

        public StudentGrade Grade { get; set; }
    }
}
