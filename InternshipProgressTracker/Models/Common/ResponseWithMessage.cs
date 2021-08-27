using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Common
{
    /// <summary>
    /// Represents response with some message
    /// </summary>
    public class ResponseWithMessage
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
