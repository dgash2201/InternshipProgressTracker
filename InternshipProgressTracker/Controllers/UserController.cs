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
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Receives data for registration and pass them to the user service register method
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
                var id = await _service.Register(registerDto, cancellationToken);

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
        /// Receives data for login and pass them to the user service login method
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
                var token = await _service.Login(loginDto, cancellationToken);

                if (token == null)
                {
                    return BadRequest(new { Success = false, Message = "login or password is incorrect" });
                }

                return Ok(new { Success = true, Token = token });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch(BadRequestException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
