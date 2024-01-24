using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionKeywordController : ControllerBase
    {
        private readonly IProfessionKeywordService _service;

        public ProfessionKeywordController(IProfessionKeywordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllProfessionKeywords")]

        public async Task<ActionResult<IEnumerable<ProfessionKeyword>>> GetProfessionKeywords()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetProfessionKeywordById{id}")]
        public async Task<ActionResult<IEnumerable<ProfessionKeyword>>> GetProfessionKeywordById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddProfessionKeyword")]
        public async Task<ActionResult<ProfessionKeyword>> AddProfessionKeyword(ProfessionKeyword newProfessionKeyword)
        {
            return Ok(await _service.Create(newProfessionKeyword));
        }

        [HttpPut]
        [Route("UpdateProfessionKeyword")]
        public IActionResult UpdateProfessionKeyword(ProfessionKeyword reqProfessionKeyword)
        {
            _service.Update(reqProfessionKeyword);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteProfessionKeywordById{id}")]
        public IActionResult DeleteProfessionKeywordById(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
