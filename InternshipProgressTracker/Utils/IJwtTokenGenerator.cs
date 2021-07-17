using Microsoft.AspNetCore.Identity;

namespace InternshipProgressTracker.Utils
{
    /// <summary>
    /// Interface for token generator
    /// </summary>
    public interface IJwtTokenGenerator
    {
        public string Generate(IdentityUser<int> user, string role);
    }
}
