using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.InternshipStreams
{
    /// <summary>
    /// Contains data for binding student with internship stream
    /// </summary>
    public class AddStudentDto
    {  
        public int InternshipStreamId { get; set; }

        public int StudentId { get; set; }
    }
}
