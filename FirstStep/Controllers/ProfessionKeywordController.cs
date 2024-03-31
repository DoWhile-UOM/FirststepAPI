using FirstStep.Models;
using FirstStep.Models.DTOs;
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
        [Route("GetAllProfessionKeywords/{fieldID:int}")]

        public async Task<ActionResult<IEnumerable<string>>> GetProfessionKeywords(int fieldID)
        {
            return Ok(await _service.GetAll(fieldID));
        }

        [HttpPost]
        [Route("AddProfessionKeyword")]

        public async Task<IActionResult> AddProfessionKeyword(ProfessionKeywordDto newProfessionKeyword)
        {
            await _service.Create(newProfessionKeyword);
            return Ok($"Successfully added profession keyword: {newProfessionKeyword.profession_name}.");
        }

        [HttpPut]
        [Route("UpdateProfessionKeyword/{id:int}")]

        public async Task<IActionResult> UpdateProfessionKeyword(ProfessionKeyword reqProfessionKeyword, int id)
        {
            if (id != reqProfessionKeyword.profession_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(id, reqProfessionKeyword);
            return Ok($"Successfully updated profession keyword: {reqProfessionKeyword.profession_name}.");
        }

        [HttpDelete]
        [Route("DeleteProfessionKeywordById/{id:int}")]

        public async Task<IActionResult> DeleteProfessionKeywordById(int id)
        {
            await _service.Delete(id);
            return Ok($"Successfully removed profession keyword: {id}.");
        }
    }
}
