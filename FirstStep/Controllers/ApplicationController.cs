using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllApplications")]

        public async Task<ActionResult<IEnumerable<Application>>>GetAllApplications()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetApplicationById{id}")]

        public async Task<ActionResult<Application>> GetApplicationById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddApplication")]

        public async Task<ActionResult<Application>> AddApplication(Application application)
        {
            return Ok(await _service.Create(application));
        }

        [HttpPut]
        [Route("UpdateCApplication")]

        public IActionResult UpdateCApplication(Application reqApplication)
        {
            _service.Update(reqApplication);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteApplication{id}")]
        public IActionResult DeleteApplication(int id)
        {
            _service.Delete(id);
            return Ok();
        }

    }
}
