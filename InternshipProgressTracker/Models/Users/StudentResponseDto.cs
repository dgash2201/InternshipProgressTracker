using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using System.Collections.Generic;

namespace InternshipProgressTracker.Models.Users
{
    /// <summary>
    /// Contains student data
    /// </summary>
    public class StudentResponseDto
    {
        public StudentGrade? CurrentGrade { get; set; }

        public ICollection<StudentProgressResponseDto> StudyPlanProgresses { get; set; }
    }
}
