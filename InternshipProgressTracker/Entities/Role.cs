using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents user role
    /// </summary>
    public class Role : IdentityRole<int>
    {
        public ICollection<User> Users { get; set; }
    }
}
