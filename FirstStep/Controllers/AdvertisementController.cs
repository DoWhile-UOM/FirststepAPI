using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ActionResult<Advertisement>> AddAdvertisement(Advertisement advertisement)
        {
            return Ok(await _service.Create(advertisement));
        }

        [HttpPut]
        [Route("UpdateAdvertisement")]

        public  IActionResult UpdateAdvertisement(Advertisement reqAdvertisement)
        {
            _service.Update(reqAdvertisement);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAdvertisement{id}")]

        public IActionResult DeleteAdvertisement(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
