using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Admins;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Admins
    /// </summary>
    [Authorize(AuthenticationSchemes = "MyBearer", Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<UserResponseDto>> { Success = true, Model = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Add admin role to user
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin(CreateAdminDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var adminDto = await _adminService.CreateAdminAsync(createDto.UserId, cancellationToken);

                return Ok(new ResponseWithModel<UserResponseDto> { Success = true, Model = adminDto });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Add mentor role to user
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create-mentor")]
        public async Task<IActionResult> CreateMentor(CreateMentorDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var mentorDto = await _adminService.CreateMentorAsync(createDto.UserId, createDto.Role, cancellationToken);

                return Ok(new ResponseWithModel<UserResponseDto> { Success = true, Model = mentorDto });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
