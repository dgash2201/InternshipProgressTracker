using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public int InternshipStreamId { get; set; }

        public int CurrentGrade { get; set; }

        public User User { get; set; }
    }
}
