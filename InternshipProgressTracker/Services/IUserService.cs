using InternshipProgressTracker.Models.User;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IUserService
    {
        Task Register(RegisterDto registerDto);
        Task Login(LoginDto loginDto);
    }
}
