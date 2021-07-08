using InternshipProgressTracker.Models.User;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services
{
    public interface IUserService
    {
        Task Register(RegisterDto registerDto);
    }
}
