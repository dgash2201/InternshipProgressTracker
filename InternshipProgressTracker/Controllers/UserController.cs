using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Users;
using InternshipProgressTracker.Exceptions;
using Microsoft.Extensions.Logging;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Users
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _userService = service;
            _logger = logger;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        /// <response code="409">User already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _userService.RegisterAsync(registerDto, cancellationToken);

                return Ok(new { Success = true, Id = id });
            }
            catch(AlreadyExistsException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Authentificate user
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        /// <response code="400">Email or password is incorrect</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var (jwt, refreshToken) = await _userService.LoginAsync(loginDto, cancellationToken);

                return Ok(new { Success = true, Jwt = jwt, RefreshToken = refreshToken });
            }
            catch(BadRequestException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Create new JWT
        /// </summary>
        /// <response code="400">Refresh token is incorrect</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("refresh-jwt")]
        public async Task<IActionResult> RefreshJwt(string refreshToken, int userId)
        {
            try
            {
                var (newJwt, newRefreshToken) = await _userService.RefreshJwtAsync(refreshToken, userId);

                return Ok(new { Succes = true, Jwt = newJwt, RefreshToken = newRefreshToken });
            }
            catch(BadRequestException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
