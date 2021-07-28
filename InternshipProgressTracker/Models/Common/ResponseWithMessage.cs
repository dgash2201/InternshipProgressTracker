using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Common
{
    public class ResponseWithMessage
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
