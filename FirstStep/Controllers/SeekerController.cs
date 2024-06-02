using FirstStep.Models;
using FirstStep.Models.DTOs;
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
        [Route("GetSeeker/{seekerId:int}")]

        public async Task<ActionResult<Seeker>> GetSeekerById(int seekerId)
        {
            return Ok(await _service.GetById(seekerId));
        }

        [HttpGet]
        [Route("GetSeekerDetails/{seekerId:int}")]
        public async Task<ActionResult<SeekerApplicationDto>> GetSeekerDetails(int seekerId)
        {
            return Ok(await _service.GetSeekerDetails(seekerId));
        }


        [HttpPost]
        [Route("AddSeeker")]

        public async Task<IActionResult> AddSeeker(AddSeekerDto newSeeker)
        {
            try
            {
                var response = await _service.Create(newSeeker);

                return response switch
                {
                    "Seeker added successfully" => Ok(response),
                    "Null User" => BadRequest(response),
                    "Email Already exist" => BadRequest(response),
                    _ => BadRequest(response),
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSeeker/{seekerId:int}")]

        public async Task<IActionResult> UpdateSeeker(Seeker reqseeker, int seekerId)
        {
            if (seekerId != reqseeker.user_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(seekerId, reqseeker);
            return Ok($"Successfully Updated SeekerID: {reqseeker.first_name} {reqseeker.last_name}");
        }

        [HttpDelete]
        [Route("DeleteSeeker/{seekerId:int}")]

        public async Task<IActionResult> DeleteSeeker(int seekerId)
        {
            await _service.Delete(seekerId);
            return Ok();
        }
    }
}
