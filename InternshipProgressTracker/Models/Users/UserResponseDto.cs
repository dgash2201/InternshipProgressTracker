using Microsoft.AspNetCore.Mvc;

namespace InternshipProgressTracker.Models.Users
{
    /// <summary>
    /// Contains user data
    /// </summary>
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public StudentResponseDto Student { get; set; }

        public MentorResponseDto Mentor { get; set; }
    }
}
