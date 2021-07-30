using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities.Enums
{
    /// <summary>
    /// Contains indepedent student grades
    /// </summary>
    public enum StudentGrade
    {
        Junior,
        Middle,
        Senior
    }

    /// <summary>
    /// Contains statuses of internship stream
    /// </summary>
    public enum InternshipStreamStatus
    {
        NotStarted,
        Active,
        Completed
    }

    /// <summary>
    /// Contains roles of mentor
    /// </summary>
    public enum MentorRole
    {
        Mentor,
        Lead
    }
}
