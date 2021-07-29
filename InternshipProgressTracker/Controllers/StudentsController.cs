using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of students
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Set student grade
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="grade">Student current grade</param>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("set-grade")]
        public async Task<IActionResult> SetStudentGrade(int studentId, StudentGrade grade)
        {
            try
            {
                await _studentService.SetStudentGradeAsync(studentId, grade);

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
        /// Mark study plan entry as started
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="entryId">Id of study plan entry</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Student or study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpPost("start-study-plan-entry")]
        public async Task<IActionResult> StartStudyPlanEntry(int entryId)
        {
            try
            {
                var studentId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _studentService.StartStudyPlanEntryAsync(studentId, entryId);

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
        /// Mark study plan entry as finished
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <param name="entryId">Id of study plan entry</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Student or study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpPut("finish-study-plan-entry")]
        public async Task<IActionResult> FinishStudyPlanEntry(int entryId)
        {
            try
            {
                var studentId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _studentService.FinishStudyPlanEntryAsync(studentId, entryId);

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
