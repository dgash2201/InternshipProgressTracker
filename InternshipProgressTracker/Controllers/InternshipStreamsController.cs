using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.InternshipStreams;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InternshipStreamsController : Controller
    {
        private readonly IInternshipStreamService _internshipStreamService;

        public InternshipStreamsController(IInternshipStreamService internshipStreamService)
        {
            _internshipStreamService = internshipStreamService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var internshipStreams = await _internshipStreamService.Get();

            return Ok(internshipStreams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var internshipStream = await _internshipStreamService.Get(id);

            return Ok(internshipStream);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _internshipStreamService.Delete(id);

                return Ok(new { Success = true });
            } 
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
