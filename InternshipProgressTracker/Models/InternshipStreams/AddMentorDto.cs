using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    /// <summary>
    /// Contains data for binding mentro with internship stream
    /// </summary>
    public class AddMentorDto
    {    
        public int InternshipStreamId { get; set; }

        public int MentorId { get; set; }
    }
}
