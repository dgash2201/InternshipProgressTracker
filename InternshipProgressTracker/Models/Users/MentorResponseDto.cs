using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using System.Collections.Generic;

namespace InternshipProgressTracker.Models.Users
{
    public class MentorResponseDto
    {
        public IReadOnlyCollection<StudentProgressResponseDto> StudentProgresses { get; set; }
    }
}
