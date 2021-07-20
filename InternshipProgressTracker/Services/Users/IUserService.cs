using System.Threading;
using System.Threading.Tasks;
using InternshipProgressTracker.Models.Users;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IUserService
    {
        Task<int> Register(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<(string, string)> Login(LoginDto loginDto, CancellationToken cancellationToken);
        Task<(string, string)> RefreshJwt(string refreshToken, int id);
    }
}
