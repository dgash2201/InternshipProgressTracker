using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.InternshipStreams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    /// <summary>
    /// Controller for working with InternshipStream entities
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class InternshipStreamsController : Controller
    {
        private readonly IInternshipStreamService _internshipStreamService;

        public InternshipStreamsController(IInternshipStreamService internshipStreamService)
        {
            _internshipStreamService = internshipStreamService;
        }

        /// <summary>
        /// Binds student with internship stream
        /// </summary>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream or student was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost("add-student")]
        public async Task<IActionResult> AddStudent(int streamId, int studentId)
        {
            try
            {
                await _internshipStreamService.AddStudent(streamId, studentId);

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
        /// Get list of internship streams
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
                var internshipStreams = await _internshipStreamService.Get();

                return Ok(internshipStreams);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Gets an internship stream by id
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Student, Mentor, Lead, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var internshipStream = await _internshipStreamService.Get(id);

                return Ok(internshipStream);
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
        /// Creates internship stream
        /// </summary>
        /// <param name="createDto">Contains data for creation</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInternshipStreamDto createDto)
        {
            try
            {
                var id = await _internshipStreamService.Create(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates internship stream data
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <param name="updateDto">New data</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInternshipStreamDto updateDto)
        {
            try
            {
                await _internshipStreamService.Update(id, updateDto);

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
        /// Deletes internship stream
        /// </summary>
        /// <param name="id">Id of the internship stream</param>
        /// <response code="401">Authorization token is invalid</response>
        /// <response code="403">Forbidden for this role</response>
        /// <response code="404">Internship stream was not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "Mentor, Lead, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _internshipStreamService.SoftDelete(id);

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
    }
}
