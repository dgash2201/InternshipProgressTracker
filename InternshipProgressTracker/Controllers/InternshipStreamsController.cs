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
        /// Binds student with InternshipStream
        /// </summary>
        [HttpPost("add-student")]
        public async Task<IActionResult> AddStudent(int streamId, int studentId)
        {
            try
            {
                await _internshipStreamService.AddStudent(streamId, studentId);

                return Ok(new { Success = true });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Get list of InternshipStreams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var internshipStreams = await _internshipStreamService.Get();

            return Ok(internshipStreams);
        }

        /// <summary>
        /// Gets an InternshipStream by id
        /// </summary>
        /// <param name="id">Id of an InternshipStream</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var internshipStream = await _internshipStreamService.Get(id);

            return Ok(internshipStream);
        }

        /// <summary>
        /// Creates InternshipStream
        /// </summary>
        /// <param name="createDto">Contains data for creation</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInternshipStreamDto createDto)
        {
            try
            {
                var id = await _internshipStreamService.Create(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Updates InternshipStream data
        /// </summary>
        /// <param name="id">InternshipStream id</param>
        /// <param name="updateDto">New data</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInternshipStreamDto updateDto)
        {
            try
            {
                await _internshipStreamService.Update(id, updateDto);

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes InternshipStream
        /// </summary>
        /// <param name="id">IntenshipStream id</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _internshipStreamService.SoftDelete(id);

                return Ok(new { Success = true });
            } 
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
