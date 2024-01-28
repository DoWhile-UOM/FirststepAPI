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

        public async Task<IActionResult> AddJobField(JobField jobField)
        {
            await _service.Create(jobField);
            return Ok($"Sucessfull added new job field: {jobField.field_name}");
        }

        [HttpPut]
        [Route("UpdateJobField/{id:int}")]

        public async Task<IActionResult> UpdateJobField(JobField reqJobField, int id)
        {
            if (id != reqJobField.field_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(reqJobField);
            return Ok($"Sucessfully Updated: Job field {reqJobField.field_name}");
        }

        [HttpDelete]
        [Route("DeleteJobFieldById/{id:int}")]

        public async Task<IActionResult> DeleteJobFieldById(int id)
        {
            await _service.Delete(id);
            return Ok($"Suncessfully Deleted: job field {id}");
        }
    }
}
