using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public class StudyPlanEntry
    {
        public int Id { get; set; }

        public int StudyPlanId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }
    }
}
