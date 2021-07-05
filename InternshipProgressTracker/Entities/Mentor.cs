using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public enum MentorRole
    {
        Mentor,
        Lead
    }

    public class Mentor
    {
        public int Id { get; set; }

        public MentorRole Role { get; set; }

        public bool IsAdmin { get; set; }

        public int InternshipStreamId { get; set; }

        public User User { get; set; }
    }
}
