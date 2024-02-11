using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _service;

        public AdvertisementController(IAdvertisementService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllAdvertisements")]

        public async Task<ActionResult<IEnumerable<AdvertisementCardDto>>> GetAdvertisements()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetAdvertisementById/{jobID:int}")]

        public async Task<ActionResult<AdvertisementDto>> GetAdvertisementById(int jobID)
        {            
            return Ok(await _service.GetById(jobID));
        }

        [HttpPost]
        [Route("AddAdvertisement")]

        public async Task<IActionResult> AddAdvertisement(AddAdvertisementDto advertisementDto)
        {
            if (advertisementDto is null)
            {
                return BadRequest("Advertisement cannot be null.");
            }

            await _service.Create(advertisementDto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateAdvertisement/{jobID:int}")]

        public async Task<IActionResult> UpdateAdvertisement(UpdateAdvertisementDto reqAdvertisement, int jobID)
        {
            if (reqAdvertisement is null)
            {
                return BadRequest("Advertisement cannot be null.");
            }

            if (jobID != reqAdvertisement.advertisement_id)
            {
                return BadRequest("Advertisement ID mismatch.");
            }

            await _service.Update(jobID, reqAdvertisement);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/{jobID:int}")]

        public async Task<IActionResult> DeleteAdvertisement(int jobID)
        {
            await _service.Delete(jobID);
            return Ok();
        }
    }
}
