using InternshipProgressTracker.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Admins
{
    public class CreateMentorDto
    {
        public int UserId { get; set; }

        public MentorRole Role { get; set; }
    }
}
