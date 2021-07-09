using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Utils;

namespace InternshipProgressTracker.Services.Users
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IJwtTokenGenerator _tokenGenerator;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!signInResult.Succeeded)
                return null;

            return _tokenGenerator.Generate(user);
        }

        public Task<int> Register(RegisterDto registerDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
