using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Study Plans
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class StudyPlansController : ControllerBase
    {
        private readonly IStudyPlanService _studyPlanService;

        public StudyPlansController(IStudyPlanService studyPlanService)
        {
            _studyPlanService = studyPlanService;
        }

        /// <summary>
        /// Get all study plans including soft deleted (only for admin)
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetWithSoftDeleted()
        {
            var studyPlans = await _studyPlanService.GetWithSoftDeletedAsync();

            return Ok(new { Success = true, StudyPlans = studyPlans });
        }

        /// <summary>
        /// Get list of study plans
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var studyPlans = await _studyPlanService.GetAsync();

            return Ok(new { Success = true, StudyPlans = studyPlans });
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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var studyPlan = await _studyPlanService.GetAsync(id);

                return Ok(new { Success = true, StudyPlan = studyPlan });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
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
        public async Task<IActionResult> Create(StudyPlanDto createDto)
        {
            try
            {
                var id = await _studyPlanService.CreateAsync(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = true, Message = ex.Message });
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudyPlanDto updateDto)
        {
            try
            {
                await _studyPlanService.UpdateAsync(id, updateDto);

                return Ok(new { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, JsonPatchDocument<StudyPlanDto> patchDocument)
        {
            try
            {
                await _studyPlanService.UpdateAsync(id, patchDocument);

                return Ok(new { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _studyPlanService.SoftDeleteAsync(id);

                return Ok(new { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
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
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                await _studyPlanService.DeleteAsync(id);

                return Ok(new { Success = true });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
        }
    }
}
