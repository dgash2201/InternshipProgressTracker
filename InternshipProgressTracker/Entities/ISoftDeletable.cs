using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Entities
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
