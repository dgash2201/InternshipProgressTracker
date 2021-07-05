using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public class Mentor
    {
        public int Id { get; set; }

        public bool IsAdmin { get; set; }

        public int InternshipStreamId { get; set; }
    }
}
