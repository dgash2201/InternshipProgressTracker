using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Services.StudyPlans;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudyPlansController : Controller
    {
        private readonly IStudyPlanService _studyPlanService;

        public StudyPlansController(IStudyPlanService studyPlanService)
        {
            _studyPlanService = studyPlanService;
        }


        // GET: api/<StudyPlanController>
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

        // GET api/<StudyPlanController>/5
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

        // POST api/<StudyPlanController>
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyPlanDto createDto)
        {
            try
            {
                var id = _studyPlanService.Create(createDto);

                return Ok(new { Success = true, Id = id });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT api/<StudyPlanController>/5
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

        // DELETE api/<StudyPlanController>/5
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
