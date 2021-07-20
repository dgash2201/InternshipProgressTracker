using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Settings
{
    /// <summary>
    /// Contains secrets of application
    /// </summary>
    public class InternshipProgressTrackerSecrets
    {
        public string ServiceApiKey { get; set; }

        public string AdminPassword { get; set; }
    }
}
