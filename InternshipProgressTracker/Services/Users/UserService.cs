using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Token;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Users
{
    /// <summary>
    /// Service for authorization and 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IStudentService _studentService;
        private readonly IPhotoManager _photoManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenGenerator tokenGenerator,
            IStudentService studentService,
            IPhotoManager photoManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _studentService = studentService;
            _photoManager = photoManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">Id of user</param>
        public async Task<UserResponseDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager
                .Users
                .Include(u => u.Student)
                .ThenInclude(s => s.StudyPlanProgresses)
                .Include(u => u.Mentor)
                .ThenInclude(m => m.StudentStudyPlanProgresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User was not found");
            }

            var responseDto = _mapper.Map<UserResponseDto>(user);
            var roles = await _userManager.GetRolesAsync(user);

            responseDto.Role = roles.FirstOrDefault();

            if (user.PhotoId != null)
            {
                responseDto.Avatar = await _photoManager.GetAsync(user.PhotoId, cancellationToken);
            }

            return await GetUserResponseDto(user);
        }

        /// <summary>
        /// Creates user entity and saves it in the database
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new AlreadyExistsException("User with this email already exists");
            }

            var user = await CreateAsync(registerDto, cancellationToken);

            if (registerDto.Avatar != null)
            {
                var photoId = await _photoManager.UploadAsync(registerDto.Avatar, cancellationToken);
                user.PhotoId = photoId;
                await _userManager.UpdateAsync(user);
            }

            return await GetUserResponseDto(user);
        }

        /// <summary>
        /// Checks login data and returns generated token
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new NotFoundException("User with this email was not found");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!signInResult.Succeeded)
            {
                throw new BadRequestException("Email or password is incorrect");
            }

            return await GetTokenPairAsync(user, cancellationToken);
        }

        /// <summary>
        /// Authenticate azure user and save him in the database
        /// </summary>
        /// <param name="userRequest">Microsoft Graph API request to get user</param>
        /// <param name="photoRequest">Microsoft Graph API request to get photo content</param>
        public async Task<TokenResponseDto> LoginByAzureAsync(
            Microsoft.Graph.IUserRequest userRequest, 
            Microsoft.Graph.IProfilePhotoContentRequest photoRequest)
        {
            var azureUser = await userRequest.GetAsync();
            var user = await _userManager.FindByEmailAsync(azureUser.Mail);

            if (user == null)
            {
                var registerDto = new RegisterDto
                {
                    Email = azureUser.Mail,
                    FirstName = azureUser.GivenName,
                    LastName = azureUser.Surname,
                };
                
                user = await CreateAsync(registerDto);

                if (azureUser.Photo != null)
                {
                    var photoId = await _photoManager.UploadAsync(photoRequest);
                    user.PhotoId = photoId;
                    await _userManager.UpdateAsync(user);
                }
            }

            return await GetTokenPairAsync(user);
        }

        /// <summary>
        /// Creates new JWT
        /// </summary>
        public async Task<TokenResponseDto> RefreshJwtAsync(string refreshToken, int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User was not found");
            }

            if (user.RefreshToken != refreshToken)
            {
                throw new BadRequestException("Refresh token is incorrect");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var newJwt = _tokenGenerator.GenerateJwt(user, userRoles.FirstOrDefault());
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenResponseDto { UserId = user.Id, Jwt = newJwt, RefreshToken = newRefreshToken };
        }

        /// <summary>
        /// Marks user as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            await _userManager.DeleteAsync(user);
        }

        private async Task<User> CreateAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
            };

            var result = registerDto.Password == null ? 
                await _userManager.CreateAsync(user) :
                await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var exceptionMessage = "";
                foreach (var error in result.Errors)
                {
                    exceptionMessage += $"{error.Code}: {error.Description}" + System.Environment.NewLine;
                }

                throw new BadRequestException(exceptionMessage);
            }

            await _studentService.CreateAsync(user, cancellationToken);
            await _userManager.AddToRoleAsync(user, "Student");

            return user;
        }

        private async Task<TokenResponseDto> GetTokenPairAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userRoles = await _userManager.GetRolesAsync(user);
            var newJwt = _tokenGenerator.GenerateJwt(user, userRoles.FirstOrDefault());
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenResponseDto { UserId = user.Id, Jwt = newJwt, RefreshToken = newRefreshToken };
        }

        private async Task<UserResponseDto> GetUserResponseDto(User user, CancellationToken cancellationToken = default)
        {
            var responseDto = _mapper.Map<UserResponseDto>(user);
            var roles = await _userManager.GetRolesAsync(user);

            responseDto.Role = roles.FirstOrDefault();

            if (user.PhotoId != null)
            {
                responseDto.Avatar = await _photoManager.GetAsync(user.PhotoId, cancellationToken);
            }

            return responseDto;
        }
    }
}
