﻿using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Token;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Utils;
using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenGenerator tokenGenerator,
            IStudentService studentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _studentService = studentService;
        }



        /// <summary>
        /// Creates user entity and saves it in the database
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        public async Task<int> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
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
                using (var memoryStream = new MemoryStream())
                {
                    await registerDto.Avatar.CopyToAsync(memoryStream);
                    user.Photo = memoryStream.ToArray();
                }

                await _userManager.UpdateAsync(user);
            }

            await _studentService.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "Student");

            return user.Id;
        }

        /// <summary>
        /// Checks login data and returns generated token
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

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

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwt = _tokenGenerator.GenerateJwt(user, userRoles.FirstOrDefault());
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new TokenResponseDto { UserId = user.Id, Jwt = jwt, RefreshToken = refreshToken };
        }

        /// <summary>
        /// Creates new JWT
        /// </summary>
        public async Task<TokenResponseDto> RefreshJwtAsync(string refreshToken, int userId)
        {
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
    }
}
