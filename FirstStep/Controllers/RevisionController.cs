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

        [HttpGet]
        [Route("GetRevisionHistory/{applicationId:int}")]
        public async Task<ActionResult<IEnumerable<RevisionHistoryDto>>> GetRevisionHistory(int applicationId)
        {
            var revisions = await _service.GetByApplicationID(applicationId);
            if (!revisions.Any())
            {
                return NotFound("No revisions found for this application.");
            }

            var revisionHistoryDtos = revisions.Select(r => new RevisionHistoryDto
            {
                revision_id = r.revision_id,
                comment = r.comment,
                status = r.status,
                created_date = r.date,
                employee_name = r.employee!.first_name + " " + r.employee!.last_name,
                employee_role = r.employee!.user_type
            });

            return Ok(revisionHistoryDtos);
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
