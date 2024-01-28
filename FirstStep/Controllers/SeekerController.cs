using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerController : ControllerBase
    {
        private readonly ISeekerService _service;

        public SeekerController(ISeekerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllSeekers")]

        public async Task<ActionResult<IEnumerable<Seeker>>> GetAllSeekers()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetSeeker/{id}")]

        public async Task<ActionResult<Seeker>> GetSeekerById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddSeeker")]

        public async Task<IActionResult> AddSeeker(Seeker seeker)
        {
            await _service.Create(seeker);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateSeeker")]

        public async Task<IActionResult> UpdateSeeker(Seeker reqseeker)
        {
            await _service.Update(reqseeker);
            return Ok($"Successfully Updated SeekerID: {reqseeker.user_id}");
        }

        [HttpDelete]
        [Route("DeleteSeeker/{id}")]

        public async Task<IActionResult> DeleteSeeker(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
