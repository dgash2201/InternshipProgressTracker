using Microsoft.AspNetCore.Identity;

namespace InternshipProgressTracker.Utils
{
    /// <summary>
    /// Interface for token generator
    /// </summary>
    public interface ITokenGenerator
    {
        public string GenerateJwt(IdentityUser<int> user, string role);
        public string GenerateRefreshToken();
    }
}
