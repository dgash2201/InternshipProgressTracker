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
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> Update(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
