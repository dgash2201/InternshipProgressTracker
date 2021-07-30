using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Common
{
    public class ResponseWithId
    {
        public bool Success { get; set; }

        public int Id { get; set; }
    }
}
