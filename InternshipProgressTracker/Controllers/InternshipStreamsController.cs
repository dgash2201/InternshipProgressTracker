using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.InternshipStreams;
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
    /// Represents Web API of Internship Streams
    /// </summary>
    [Authorize(AuthenticationSchemes = "MyBearer")]
    [ApiController]
    [Route("[controller]")]
    public class InternshipStreamsController : Controller
    {
        private readonly IInternshipStreamService _internshipStreamService;
        private readonly ILogger<IInternshipStreamService> _logger;

        public InternshipStreamsController(IInternshipStreamService internshipStreamService, ILogger<InternshipStreamService> logger)
        {
            _internshipStreamService = internshipStreamService;
            _logger = logger;
        }

        /// <summary>
        /// Get all internship stream including soft deleted (only for admin)
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetWithSoftDeleted(CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreams = await _internshipStreamService.GetWithSoftDeletedAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<InternshipStreamResponseDto>> { Success = true, Model = internshipStreams });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Bind mentor with internship stream
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream or mentor was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("add-mentor")]
        public async Task<IActionResult> AddMentor(InternshipStreamMentorDto addMentorDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService
                    .AddMentorAsync(addMentorDto.InternshipStreamId, addMentorDto.MentorId, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Bind student with internship stream
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream or student was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("add-student")]
        public async Task<IActionResult> AddStudent(InternshipStreamStudentDto addStudentDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService
                    .AddStudentAsync(addStudentDto.InternshipStreamId, addStudentDto.StudentId, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Remove mentor from internship stream
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream or mentor was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("remove-mentor")]
        public async Task<IActionResult> RemoveMentor(InternshipStreamMentorDto removeMentorDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService
                    .RemoveMentorAsync(removeMentorDto.InternshipStreamId, removeMentorDto.MentorId, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Remove student from internship stream
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream or student was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("remove-student")]
        public async Task<IActionResult> RemoveStudent(InternshipStreamStudentDto removeStudentDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService
                    .RemoveStudentAsync(removeStudentDto.InternshipStreamId, removeStudentDto.StudentId, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Get list of internship streams
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(int? studentId, int? mentorId, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreams = await _internshipStreamService.GetAsync(studentId, mentorId, cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<InternshipStreamResponseDto>> { Success = true, Model = internshipStreams });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get an internship stream by id
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStream = await _internshipStreamService.GetAsync(id, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStream });
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
        /// Create internship stream
        /// </summary>
        /// <param name="createDto">Contains data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(InternshipStreamDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService.CreateAsync(createDto, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Update internship stream data
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <param name="updateDto">New data</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(PutRequestDto<InternshipStreamDto> putRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService.UpdateAsync(putRequestDto.Id, putRequestDto.Model, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Patch internship stream data
        /// </summary>
        /// <param name="id">Id of internship stream</param>
        /// <param name="patchDocument">JsonPatch operations</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPatch]
        public async Task<IActionResult> Update(PatchRequestDto<InternshipStreamDto> patchDto, CancellationToken cancellationToken)
        {
            try
            {
                var internshipStreamResponseDto = await _internshipStreamService.UpdateAsync(patchDto.Id, patchDto.PatchDocument, cancellationToken);

                return Ok(new ResponseWithModel<InternshipStreamResponseDto> { Success = true, Model = internshipStreamResponseDto });
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
        /// Mark internship stream as deleted
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _internshipStreamService.SoftDeleteAsync(id, cancellationToken);

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
        /// Delete internship stream (only for admin)
        /// </summary>
        /// <param name="id">Id of internship stream</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("hard-delete/{id}")]
        public async Task<IActionResult> HardDelete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _internshipStreamService.DeleteAsync(id, cancellationToken);

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
