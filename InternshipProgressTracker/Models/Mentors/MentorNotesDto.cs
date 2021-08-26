using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Mentors
{
    public class MentorNotesDto
    {
        public int StudentId { get; set; }

        public int StudyPlanEntryId { get; set; }

        public string Notes { get; set; }
    }
}
