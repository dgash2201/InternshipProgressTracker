using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Study Plans
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class StudyPlansController : Controller
    {
        private readonly IStudyPlanService _studyPlanService;

        public StudyPlansController(IStudyPlanService studyPlanService)
        {
            _studyPlanService = studyPlanService;
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
            try
            {
                var studyPlans = await _studyPlanService.Get();

                return Ok(new { Success = true, StudyPlans = studyPlans });
            }
            catch
            {
                return StatusCode(500);
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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var studyPlan = await _studyPlanService.Get(id);

                return Ok(new { Success = true, StudyPlan = studyPlan });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Create study plan
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyPlanDto createDto)
        {
            try
            {
                var id = await _studyPlanService.Create(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update study plan data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="updateDto">New data</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateStudyPlanDto updateDto)
        {
            try
            {
                await _studyPlanService.Update(id, updateDto);

                return Ok(new { Success = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500);
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
                await _studyPlanService.SoftDelete(id);

                return Ok(new { Succes = true });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
