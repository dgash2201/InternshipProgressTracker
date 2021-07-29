using System.Threading;
using System.Threading.Tasks;
using InternshipProgressTracker.Models.Token;
using InternshipProgressTracker.Models.Users;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IUserService
    {
        Task<int> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<TokenResponseDto> RefreshJwtAsync(string refreshToken, int id);
    }
}
