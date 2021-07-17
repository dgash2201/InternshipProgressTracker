using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Utils;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Exceptions;
using System.Linq;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Service for authorization and 
    /// </summary>
    public class UserService : IUserService
    {
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
                throw new NotFoundException("User with this email was not found");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!signInResult.Succeeded)
                throw new BadRequestException("Email or password is incorrect");

            var userRoles = await _userManager.GetRolesAsync(user);

            return _tokenGenerator.Generate(user, userRoles.FirstOrDefault());
        }

        /// <summary>
        /// Creates user entity and saves it in the database
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        public async Task<int> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new AlreadyExistsException("User with this email already exists");
            }

            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new System.Exception("Failed to create a user");
            }

            if (registerDto.Avatar != null)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await registerDto.Avatar.CopyToAsync(memoryStream);
                    user.Photo = memoryStream.ToArray();
                }

                await _userManager.UpdateAsync(user);
            }

            await _studentService.Create(user);

            await _userManager.AddToRoleAsync(user, "Student");

            return user.Id;
        }
    }
}
