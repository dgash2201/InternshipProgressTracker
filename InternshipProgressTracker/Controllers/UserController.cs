using InternshipProgressTracker.Models.User;
using InternshipProgressTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            await _service.Register(registerDto);

            return Ok(new { Success = true });
        }

        /// <summary>
        /// Receives data for login and pass them to the user service login method
        /// </summary>
        /// <param name="loginDto">Contains login form data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            await _service.Login(loginDto);

            return Ok(new { Success = true });
        }
    }
}
