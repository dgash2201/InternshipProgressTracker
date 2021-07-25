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
        Task<int> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<(string, string)> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<(string, string)> RefreshJwtAsync(string refreshToken, int id);
    }
}
