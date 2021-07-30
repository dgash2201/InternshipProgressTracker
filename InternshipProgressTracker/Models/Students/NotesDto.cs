using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Students
{
    /// <summary>
    /// Contains data for adding notes
    /// </summary>
    public class NotesDto
    {
        public int StudyPlanEntryId { get; set; }

        public string Notes { get; set; }
    }
}
