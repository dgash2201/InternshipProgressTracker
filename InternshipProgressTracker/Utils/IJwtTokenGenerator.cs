using Microsoft.AspNetCore.Identity;

namespace InternshipProgressTracker.Utils
{
    public interface IJwtTokenGenerator
    {
        public string Generate(IdentityUser<int> user);
    }
}
