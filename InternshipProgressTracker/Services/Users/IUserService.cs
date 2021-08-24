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
        Task<UserResponseDto> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<UserResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
        Task<TokenResponseDto> LoginByAzureAsync(Microsoft.Graph.IUserRequest azureUserRequest, Microsoft.Graph.IProfilePhotoContentRequest photoRequest);
        Task<TokenResponseDto> RefreshJwtAsync(string refreshToken, int id, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
