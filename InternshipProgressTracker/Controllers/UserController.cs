using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Users;

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
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _service.Register(registerDto, cancellationToken);

                return Ok(new { Success = true, Id = id });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Receives data for login and pass them to the user service login method
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        /// <returns></returns>
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
            catch(Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
