using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Utils;
using InternshipProgressTracker.Services.Students;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Service for authorization and 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly string _avatarsPath = Directory.GetCurrentDirectory() + "/SourceData/Avatars/";
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IStudentService _studentService;

        public UserService(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtTokenGenerator tokenGenerator,
            IStudentService studentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _studentService = studentService;
        }

        /// <summary>
        /// Checks login data and returns generated token
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        public async Task<string> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!signInResult.Succeeded)
                return null;

            return _tokenGenerator.Generate(user);
        }

        /// <summary>
        /// Creates user entity and saves it in the database
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        public async Task<(int, int)> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = $"{registerDto.FirstName}_{registerDto.LastName}",
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new System.Exception("Failed to create a user");
            }

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

            var studentId = await _studentService.Create(user);

            return (user.Id, studentId);
        }
    }
}
