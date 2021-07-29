using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Token
{
    /// <summary>
    /// DTO with JWT and refresh token
    /// </summary>
    public class TokenResponseDto
    {
        public string Jwt { get; set; }

        public string RefreshToken { get; set; }
    }
}
