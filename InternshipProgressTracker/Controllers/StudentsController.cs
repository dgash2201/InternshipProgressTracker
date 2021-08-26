using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using InternshipProgressTracker.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of students
    /// </summary>
    [Authorize(AuthenticationSchemes = "MyBearer")]
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
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("set-grade")]
        public async Task<IActionResult> SetStudentGrade(StudentGradeDto gradeDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studentService.SetStudentGradeAsync(gradeDto.StudentId, gradeDto.Grade, cancellationToken);

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
        /// Set grade to student progress on study plan entry
        /// </summary>
        /// <response code="400">Study plan entry was not finished by the student</response>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Student, mentor or study plan entry was not found</response>
        /// <response code="409">Student already has grade on this study plan entry</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("grade-progress")]
        public async Task<IActionResult> GradeStudentProgress(GradeProgressDto gradeProgressDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studentService.GradeStudentProgressAsync(gradeProgressDto, cancellationToken);

                return Ok(new Response { Success = true });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ResponseWithMessage { Success = false, Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseWithMessage { Success = false, Message = ex.Message });
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
        /// Add notes
        /// </summary>
        /// <response code="400">Study plan entry was not started by the student</response>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Studentor study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpPut("add-notes")]
        public async Task<IActionResult> AddNotes(NotesDto notesDto, CancellationToken cancellationToken)
        {
            try
            {
                var studentId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _studentService.AddNotesAsync(studentId, notesDto, cancellationToken);

                return Ok(new Response { Success = true });
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
        /// Mark study plan entry as started
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Student or study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpPost("start-study-plan-entry")]
        public async Task<IActionResult> StartStudyPlanEntry(ProgressDto progressDto, CancellationToken cancellationToken)
        {
            try
            {
                var studentId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var studentProgressResponseDto = await _studentService
                    .StartStudyPlanEntryAsync(studentId, progressDto.StudyPlanEntryId, cancellationToken);

                return Ok(new ResponseWithModel<StudentProgressResponseDto> { Success = true, Model = studentProgressResponseDto });
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
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Student or study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpPut("finish-study-plan-entry")]
        public async Task<IActionResult> FinishStudyPlanEntry(ProgressDto progressDto, CancellationToken cancellationToken)
        {
            try
            {
                var studentId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var studentProgressResponseDto = await _studentService
                    .FinishStudyPlanEntryAsync(studentId, progressDto.StudyPlanEntryId, cancellationToken);

                return Ok(new ResponseWithModel<StudentProgressResponseDto> { Success = true, Model = studentProgressResponseDto });
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
    }
}
