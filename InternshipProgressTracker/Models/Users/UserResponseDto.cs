using Microsoft.AspNetCore.Mvc;

namespace InternshipProgressTracker.Models.Users
{
    public class UserResponseDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public FileContentResult Avatar { get; set; }

        public string Email { get; set; }
    }
}
