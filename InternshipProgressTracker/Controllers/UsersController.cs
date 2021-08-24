using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.Token;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Users
    /// </summary> 
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IOptions<MicrosoftGraphOptions> _graphOptions;

        public UsersController(IUserService service, ILogger<UsersController> logger, ITokenAcquisition tokenAcquisition, GraphServiceClient graphServiceClient, IOptions<MicrosoftGraphOptions> graphOptions)
        {
            _userService = service;
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _graphServiceClient = graphServiceClient;
            _graphOptions = graphOptions;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Id of user</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(AuthenticationSchemes = "MyBearer", Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var responseDto = await _userService.GetAsync(id, cancellationToken);

                return Ok(new ResponseWithModel<UserResponseDto> { Success = true, Model = responseDto });
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
        /// Register new user
        /// </summary>
        /// <param name="registerDto">Contains signup form data</param>
        /// <response code="400">Incorrect registration data</response>
        /// <response code="409">User already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var userResponseDto = await _userService.RegisterAsync(registerDto, cancellationToken);

                return Ok(new ResponseWithModel<UserResponseDto> { Success = true, Model = userResponseDto });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var tokenPair = await _userService.LoginAsync(loginDto, cancellationToken);

                return Ok(new ResponseWithModel<TokenResponseDto> { Success = true, Model = tokenPair });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ResponseWithMessage { Success = false, Message = ex.Message });
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
        /// Authenticate user with Azure token
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("login-by-azure")]
        [RequiredScope("access_as_user")]
        public async Task<IActionResult> LoginByAzure()
        {
            try
            {
                var azureUserRequest = _graphServiceClient.Me.Request();
                var photoRequest =  _graphServiceClient.Me.Photo.Content.Request();
                var tokenPair = await _userService.LoginByAzureAsync(azureUserRequest, photoRequest);

                return Ok(new ResponseWithModel<TokenResponseDto> { Success = true, Model = tokenPair });
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
        public async Task<IActionResult> RefreshJwt(string refreshToken, int userId, CancellationToken cancellationToken)
        {
            try
            {
                var tokenPair = await _userService.RefreshJwtAsync(refreshToken, userId, cancellationToken);

                return Ok(new ResponseWithModel<TokenResponseDto> { Success = true, Model = tokenPair });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ResponseWithMessage { Success = false, Message = ex.Message });
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
        /// Mark user as deleted
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(AuthenticationSchemes = "MyBearer", Roles = "Student, Mentor, Lead, Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _userService.SoftDeleteAsync(userId, cancellationToken);

                return Ok(new Response { Success = true });
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
        /// Delete user (only for admin)
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">User was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(AuthenticationSchemes = "MyBearer", Roles = "Admin")]
        [HttpDelete("hard-delete/{id}")]
        public async Task<IActionResult> HardDelete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.DeleteAsync(id, cancellationToken);

                return Ok(new Response { Success = true });
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
