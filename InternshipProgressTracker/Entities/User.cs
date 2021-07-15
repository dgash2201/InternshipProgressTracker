using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InternshipProgressTracker.Entities
{
    /// <summary>
    /// Represents the User entity
    /// </summary>
    public class User : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhotoLink { get; set; }

        public Student Student { get; set; }

        public Mentor Mentor { get; set; }
    }
}
