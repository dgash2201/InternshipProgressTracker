using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Services.StudyPlanEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    public class StudyPlanEntriesController : ControllerBase
    {
        private readonly IStudyPlanEntryService _studyPlanEntryService;

        public StudyPlanEntriesController(IStudyPlanEntryService studyPlanEntryService)
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
                var studyPlanEntries = await _studyPlanEntryService.GetAsync();

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
                var studyPlanEntry = await _studyPlanEntryService.GetAsync(id);

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
        /// <response code="404">Related study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StudyPlanEntryDto createDto)
        {
            try
            {
                var id = await _studyPlanEntryService.CreateAsync(createDto);

                return Ok(new { Success = true, Id = id });
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
        public async Task<IActionResult> Update(int id, StudyPlanEntryDto updateDto)
        {
            try
            {
                await _studyPlanEntryService.UpdateAsync(id, updateDto);

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
        /// Patch study plan entry data
        /// </summary>
        /// <param name="id">Id of study plan entry </param>
        /// <param name="patchDocument">JsonPatch operations</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Study plan entry or related study plan was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, JsonPatchDocument<StudyPlanEntryDto> patchDocument)
        {
            try
            {
                await _studyPlanEntryService.UpdateAsync(id, patchDocument);

                return Ok(new { Success = true });
            }
            catch (NotFoundException ex)
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
                await _studyPlanEntryService.SoftDeleteAsync(id);

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
