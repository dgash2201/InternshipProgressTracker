using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Services.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Admins
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService; 

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();

            return Ok(new { Success = true, Users = users });
        }

        /// <summary>
        /// Set user role
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="role">Name of role</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("set-user-role")]
        public async Task<IActionResult> SetUserRole(int userId, string role)
        {
            try
            {
                await _adminService.SetUserRoleAsync(userId, role);

                return Ok(new { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
        }
    }
}
