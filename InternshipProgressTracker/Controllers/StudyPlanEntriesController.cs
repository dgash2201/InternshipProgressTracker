using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Services.StudyPlanEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Represents Web API of Study Plan Entries
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class StudyPlansEntriesController : ControllerBase
    {
        private readonly IStudyPlanEntryService _studyPlanEntryService;

        public StudyPlansEntriesController(IStudyPlanEntryService studyPlanEntryService)
        {
            _studyPlanEntryService = studyPlanEntryService;
        }

        /// <summary>
        /// Get list of study plan entries
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
                var studyPlanEntries = await _studyPlanEntryService.Get();

                return Ok(new { Success = true, StudyPlanEntries = studyPlanEntries });
            }
            catch
            {
                return StatusCode(500);
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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var studyPlanEntry = await _studyPlanEntryService.Get(id);

                return Ok(new { Success = true, StudyPlanEntry = studyPlanEntry });
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
        /// Create study plan entry
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyPlanEntryDto createDto)
        {
            try
            {
                var id = await _studyPlanEntryService.Create(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch
            {
                return StatusCode(500);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateStudyPlanEntryDto updateDto)
        {
            try
            {
                await _studyPlanEntryService.Update(id, updateDto);

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
        /// Mark study plan entry as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _studyPlanEntryService.SoftDelete(id);

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
    }
}
