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

        //[HttpPost]
        //[Route("AddRevision")]

        //public async Task<IActionResult> AddRevision(Revision newRevision)
        //{
        //    await _service.Create(newRevision);
        //    return Ok();
        //}

        [HttpPost]
        [Route("AddRevision")]
        public async Task<IActionResult> AddRevision([FromBody] Revision newRevision)
        {
            try
            {
                await _service.AddRevision(newRevision);
                return Ok("Revision added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
