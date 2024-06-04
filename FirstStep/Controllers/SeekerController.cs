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
        [Route("GetSeekerProfile/{seekerId:int}")]
        public async Task<ActionResult<UpdateSeekerDto>> GetSeekerProfile(int seekerId)
        {
            return Ok(await _service.GetSeekerProfile(seekerId));
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
            //var result=await _service.Create(newSeeker);

            //return Ok($"Suessfully added new seeker: {newSeeker.first_name} {newSeeker.last_name}"+ result);

            try
            {
                var response = await _service.Create(newSeeker);// UserRegRequestDto must modify 

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
        public async Task<IActionResult> UpdateSeeker(int seekerId, UpdateSeekerDto updateDto)
        {
            await _service.Update(seekerId, updateDto);
            return Ok();
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
