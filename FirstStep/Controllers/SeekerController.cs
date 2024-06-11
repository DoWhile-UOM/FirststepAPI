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
        public async Task<ActionResult<SeekerProfileViewDto>> GetSeekerDetailsForSeekerProfileView(int seekerId)
        {
            try
            {
                var seekerProfileViewDto = await _service.GetSeekerDetailsForSeekerProfileView(seekerId);
                return Ok(seekerProfileViewDto);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSeekerProfile/Update/{seekerId:int}")]
        public async Task<ActionResult<UpdateSeekerDto>> GetSeekerProfile(int seekerId)
        {
            var seekerProfile = await _service.GetSeekerProfileById(seekerId);
            if (seekerProfile == null)
            {
                return NotFound("Seeker not found");
            }
            return Ok(seekerProfile);
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
                await _service.Create(newSeeker);
                return Ok(newSeeker);
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status204NoContent, e.Message);
            }
            catch (InvalidDataException e)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSeeker/{seekerId:int}")]
        public async Task<IActionResult> UpdateSeeker(int seekerId, UpdateSeekerDto updateDto)
        {
            try
            {
                await _service.Update(seekerId, updateDto);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
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
