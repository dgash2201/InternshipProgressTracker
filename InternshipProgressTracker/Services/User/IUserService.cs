using InternshipProgressTracker.Models.User;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.User
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
