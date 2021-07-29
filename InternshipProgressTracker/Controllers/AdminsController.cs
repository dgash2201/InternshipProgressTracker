using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Services.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(IAdminService adminService, ILogger<AdminsController> logger)
        {
            _adminService = adminService;
            _logger = logger;
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
            try
            {
                var users = await _adminService.GetAllUsersAsync();

                return Ok(new ResponseWithModel<IReadOnlyCollection<User>> { Success = true, Model = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
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

                return Ok(new Response { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
        }
    }
}
