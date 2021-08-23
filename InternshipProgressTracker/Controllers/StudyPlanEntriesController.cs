using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Services.StudyPlanEntries;
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
    /// Represents Web API of Study Plan Entries
    /// </summary>
    [Authorize(AuthenticationSchemes = "MyBearer")]
    [ApiController]
    [Route("[controller]")]
    public class StudyPlanEntriesController : ControllerBase
    {
        private readonly IStudyPlanEntryService _studyPlanEntryService;
        private readonly ILogger<StudyPlanEntriesController> _logger;

        public StudyPlanEntriesController(IStudyPlanEntryService studyPlanEntryService, ILogger<StudyPlanEntriesController> logger)
        {
            _studyPlanEntryService = studyPlanEntryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all study plan entries including soft deleted (only for admin)
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
                var studyPlanEntries = await _studyPlanEntryService.GetWithSoftDeletedAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<StudyPlanEntryResponseDto>> { Success = true, Model = studyPlanEntries });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get list of study plan entries
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var studyPlanEntries = await _studyPlanEntryService.GetAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<StudyPlanEntryResponseDto>> { Success = true, Model = studyPlanEntries });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get study plan entry by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var studyPlanEntry = await _studyPlanEntryService.GetAsync(id, cancellationToken);

                return Ok(new ResponseWithModel<StudyPlanEntryResponseDto> { Success = true, Model = studyPlanEntry });
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
        /// Create study plan entry
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Related study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StudyPlanEntryDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _studyPlanEntryService.CreateAsync(createDto, cancellationToken);

                return Ok(new ResponseWithId { Success = true, Id = id });
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
        /// Update study plan entry data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="updateDto">New data</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(PutRequestDto<StudyPlanEntryDto> putRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanEntryService.UpdateAsync(putRequestDto.Id, putRequestDto.Model, cancellationToken);

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
        /// Patch study plan entry data
        /// </summary>
        /// <param name="id">Id of study plan entry </param>
        /// <param name="patchDocument">JsonPatch operations</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry or related study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPatch]
        public async Task<IActionResult> Update(PatchRequestDto<StudyPlanEntryDto> patchRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanEntryService.UpdateAsync(patchRequestDto.Id, patchRequestDto.PatchDocument, cancellationToken);

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
        /// Mark study plan entry as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <response code="400">Study plan entry is already finished by one of the students</response>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanEntryService.SoftDeleteAsync(id, cancellationToken);

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
        /// Delete study plan entry (only for admin)
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
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
                await _studyPlanEntryService.DeleteAsync(id, cancellationToken);

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
