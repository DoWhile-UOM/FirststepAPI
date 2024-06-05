using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisionController : ControllerBase
    {
        private readonly IRevisionService _service;

        public RevisionController(IRevisionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllRevisions")]

        public async Task<ActionResult<IEnumerable<Revision>>> GetRevisions()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        [Route("CreateRevision")]

        public async Task<IActionResult> CreateRevision(Revision reqRevision)
        {
            await _service.Create(reqRevision);
            return Ok();
        }

      
        [HttpPut]
        [Route("UpdateRevision")]

        public async Task<IActionResult> UpdateRevision(Revision reqRevision)
        {
            await _service.Update(reqRevision);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRevisionById/{id}")]

        public async Task<IActionResult> DeleteRevisionById(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
