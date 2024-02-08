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

        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisements()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetAdvertisementById/{jobID:int}")]

        public async Task<ActionResult<Advertisement>> GetAdvertisementById(int jobID)
        {            
            return Ok(await _service.GetById(jobID));
        }

        [HttpPost]
        [Route("AddAdvertisement")]

        public async Task<IActionResult> AddAdvertisement(AddAdvertisementDto advertisementDto)
        {
            await _service.Create(advertisementDto);
            return Ok($"Sucessfully added new advertisement: {advertisementDto.title}");
        }

        [HttpPut]
        [Route("UpdateAdvertisement/{jobID:int}")]

        public async Task<IActionResult> UpdateAdvertisement(UpdateAdvertisementDto reqAdvertisement, int jobID)
        {
            await _service.Update(jobID, reqAdvertisement);
            return Ok($"Sucessfully updated advertisement: {reqAdvertisement.title}");
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/{jobID:int}")]

        public async Task<IActionResult> DeleteAdvertisement(int jobID)
        {
            await _service.Delete(jobID);
            return Ok($"Successfully deleted advertisement: {jobID}");
        }
    }
}
