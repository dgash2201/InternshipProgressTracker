using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Utils;

namespace InternshipProgressTracker.Services.Users
{
    public class UserService : IUserService
    {
        private readonly string _avatarsPath = Directory.GetCurrentDirectory() + "/SourceData/Avatars/";
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

            if (user == null)
                return null;

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!signInResult.Succeeded)
                return null;

            return _tokenGenerator.Generate(user);
        }

        public async Task<int> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = $"{registerDto.FirstName}_{registerDto.LastName}",
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (registerDto.Avatar != null)
            {
                if (!Directory.Exists(_avatarsPath))
                    Directory.CreateDirectory(_avatarsPath);

                var avatarPath = _avatarsPath + $"{user.Id}_{registerDto.Avatar.FileName}";

                using(var fileStream = new FileStream(avatarPath, FileMode.Create))
                {
                    await registerDto.Avatar.CopyToAsync(fileStream);
                }

                user.PhotoLink = avatarPath;

                await _userManager.UpdateAsync(user);
            }

            return user.Id;
        }
    }
}
