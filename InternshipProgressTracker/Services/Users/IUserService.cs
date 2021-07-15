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
        Task<(int, int)> Register(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<string> Login(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
