using FirstStep.Data;
using FirstStep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionKeywordController : ControllerBase
    {
        private readonly DataContext _context;        

        public ProfessionKeywordController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllProfessionKeywords")]

        public async Task<ActionResult<IEnumerable<ProfessionKeyword>>> GetProfessionKeywords()
        {
            return Ok(await _context.ProfessionKeywords.ToListAsync());
        }

        [HttpGet]
        [Route("GetProfessionKeywordById{id}")]
        public async Task<ActionResult<IEnumerable<ProfessionKeyword>>> GetProfessionKeywordById(int id)
        {
            return Ok(await _context.ProfessionKeywords.FindAsync(id));
        }

        [HttpPost]
        [Route("AddProfessionKeyword")]
        public async Task<ActionResult<ProfessionKeyword>> AddProfessionKeyword(ProfessionKeyword professionKeyword)
        {
            professionKeyword.profession_id = 0;

            _context.ProfessionKeywords.Add(professionKeyword);
            await _context.SaveChangesAsync();

            return Ok(await _context.ProfessionKeywords.ToListAsync());
        }

        [HttpPut]
        [Route("UpdateProfessionKeyword")]
        public async Task<IActionResult> UpdateProfessionKeyword(ProfessionKeyword reqProfessionKeyword)
        {
            
            var dbProfessionKeyword = await _context.ProfessionKeywords.FindAsync(reqProfessionKeyword.profession_id);
            if (dbProfessionKeyword == null)
            {
                return BadRequest("ProfessionKeyword not found.");
            }

            dbProfessionKeyword.profession_name = reqProfessionKeyword.profession_name;

            await _context.SaveChangesAsync();

            return Ok(await _context.ProfessionKeywords.ToListAsync());
        }

        [HttpDelete]
        [Route("DeleteProfessionKeywordById{id}")]
        public async Task<IActionResult> DeleteProfessionKeywordById(int id)
        {
            var dbProfessionKeyword = await _context.ProfessionKeywords.FindAsync(id);
            if (dbProfessionKeyword == null)
            {
                return BadRequest("ProfessionKeyword not found.");
            }

            _context.ProfessionKeywords.Remove(dbProfessionKeyword);
            await _context.SaveChangesAsync();

            return Ok(await _context.ProfessionKeywords.ToListAsync());
        }
    }
}
