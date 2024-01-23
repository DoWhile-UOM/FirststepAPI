using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobFieldController : ControllerBase
    {
        private readonly IJobFieldService _service;

        public JobFieldController(IJobFieldService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllJobFields")]

        public async Task<ActionResult<IEnumerable<JobField>>> GetJobFields()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        [Route("AddJobField")]

        public async Task<ActionResult<JobField>> AddJobField(JobField jobField)
        {
            return Ok(await _service.Create(jobField));
        }

        [HttpPut]
        [Route("UpdateJobField")]

        public IActionResult UpdateJobField(JobField reqJobField)
        {
            _service.Update(reqJobField);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteJobFieldById{id}")]

        public IActionResult DeleteJobFieldById(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
