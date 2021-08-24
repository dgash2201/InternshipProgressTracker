using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Common;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
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
    /// Represents Web API of Study Plans
    /// </summary>
    [Authorize(AuthenticationSchemes = "MyBearer")]
    [ApiController]
    [Route("[controller]")]
    public class StudyPlansController : ControllerBase
    {
        private readonly IStudyPlanService _studyPlanService;
        private readonly ILogger<StudyPlansController> _logger;

        public StudyPlansController(IStudyPlanService studyPlanService, ILogger<StudyPlansController> logger)
        {
            _studyPlanService = studyPlanService;
            _logger = logger;
        }

        /// <summary>
        /// Get all study plans including soft deleted (only for admin)
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
                var studyPlans = await _studyPlanService.GetWithSoftDeletedAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<StudyPlanResponseDto>> { Success = true, Model = studyPlans });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get list of study plans
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
                var studyPlans = await _studyPlanService.GetAsync(cancellationToken);

                return Ok(new ResponseWithModel<IReadOnlyCollection<StudyPlanResponseDto>> { Success = true, Model = studyPlans });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get study plan by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var studyPlan = await _studyPlanService.GetAsync(id, cancellationToken);

                return Ok(new ResponseWithModel<StudyPlanResponseDto> { Success = true, Model = studyPlan });
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
        /// Create study plan
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Related internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StudyPlanDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var studyPlanResponseDto = await _studyPlanService.CreateAsync(createDto, cancellationToken);

                return Ok(new ResponseWithModel<StudyPlanResponseDto> { Success = true, Model = studyPlanResponseDto });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseWithMessage { Success = true, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Update study plan data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="updateDto">New data</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan or related internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(PutRequestDto<StudyPlanDto> putRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanService.UpdateAsync(putRequestDto.Id, putRequestDto.Model, cancellationToken);

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
        /// Patch study plan data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="patchDocument">JsonPatch operations</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan or related internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPatch]
        public async Task<IActionResult> Update(PatchRequestDto<StudyPlanDto> patchRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanService.UpdateAsync(patchRequestDto.Id, patchRequestDto.PatchDocument, cancellationToken);

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
        /// Mark study plan as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _studyPlanService.SoftDeleteAsync(id, cancellationToken);

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
        /// Delete study plan (only for admin)
        /// </summary>
        /// <param name="id">Id of study plan</param>
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
                await _studyPlanService.DeleteAsync(id, cancellationToken);

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
