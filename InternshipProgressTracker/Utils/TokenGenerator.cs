using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InternshipProgressTracker.Utils
{
    /// <summary>
    /// Class for generating JWT tokens
    /// </summary>
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates JWT token
        /// </summary>
        /// <param name="user">Object with user data</param>
        public string GenerateJwt(IdentityUser<int> user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["InternshipProgressTracker:ServiceApiKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates refresh token
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
