using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.Mentors;
using InternshipProgressTracker.Models.Students;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Mentors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    public class MentorsController : ControllerBase
    {
        private readonly IMentorService _mentorService;
        private readonly ILogger<MentorsController> _logger;

        public MentorsController(IMentorService mentorService, ILogger<MentorsController> logger)
        {
            _mentorService = mentorService;
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
                var userResponseDto = await _mentorService.SetStudentGradeAsync(gradeDto.StudentId, gradeDto.Grade, cancellationToken);

                return Ok(new ResponseWithModel<UserResponseDto> { Success = true, Model = userResponseDto });
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
                var studentProgressResponseDto = await _mentorService.GradeStudentProgressAsync(gradeProgressDto, cancellationToken);

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
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("add-notes")]
        public async Task<IActionResult> AddNotes(MentorNotesDto notesDto, CancellationToken cancellationToken)
        {
            try
            {
                var studentProgressResponseDto = await _mentorService.AddNotesAsync(notesDto, cancellationToken);

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
