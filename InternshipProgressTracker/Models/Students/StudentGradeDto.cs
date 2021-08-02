using InternshipProgressTracker.Entities.Enums;

namespace InternshipProgressTracker.Models.Students
{
    /// <summary>
    /// Contains data that is needed in order to give a student a grade
    /// </summary>
    public class StudentGradeDto
    {
        public int StudentId { get; set; }

        public StudentGrade Grade { get; set; }
    }
}
