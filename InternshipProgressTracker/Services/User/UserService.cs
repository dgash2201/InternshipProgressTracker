using InternshipProgressTracker.Models.User;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.User
{
    public class UserService : IUserService
    {
        public Task<string> Login(LoginDto loginDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Register(RegisterDto registerDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
