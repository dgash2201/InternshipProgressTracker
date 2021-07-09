using InternshipProgressTracker.Models.Users;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IUserService
    {
        Task<int> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}
