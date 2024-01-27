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
        [Route("GetAdvertisementById{id}")]

        public async Task<ActionResult<Advertisement>> GetAdvertisementById(int id)
        {            
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddAdvertisement")]

        public async Task<IActionResult> AddAdvertisement(AddAdvertisementDto advertisementDto)
        {
            await _service.Create(advertisementDto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateAdvertisement")]

        public async Task<IActionResult> UpdateAdvertisement(Advertisement reqAdvertisement)
        {
            await _service.Update(reqAdvertisement);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAdvertisement{id}")]

        public async Task<IActionResult> DeleteAdvertisement(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
