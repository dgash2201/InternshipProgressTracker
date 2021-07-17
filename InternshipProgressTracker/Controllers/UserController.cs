using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Users;
using InternshipProgressTracker.Exceptions;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Controller for authentication and working with User entities
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        /// <summary>
        /// Registers new user
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
                var id = await _userService.Register(registerDto, cancellationToken);

                return Ok(new { Success = true, Id = id });
            }
            catch(AlreadyExistsException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Checks user authenfication data
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
                var (jwt, refreshToken) = await _userService.Login(loginDto, cancellationToken);

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
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Cretes new jwt
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
                var (newJwt, newRefreshToken) = await _userService.RefreshJwt(refreshToken, userId);

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
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
