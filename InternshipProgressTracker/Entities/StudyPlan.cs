﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public class StudyPlan
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int InternshipStreamId { get; set; }

        public ICollection<StudyPlanEntry> Entries { get; set; }
    }
}
