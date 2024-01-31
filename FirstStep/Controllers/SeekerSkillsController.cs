using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerSkillsController : ControllerBase
    {
        private readonly ISeekerSkillService _service;

        public SeekerSkillsController(ISeekerSkillService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllSeekerSkills")]

        public async Task<ActionResult<IEnumerable<SeekerSkill>>> GetSeekerSkills()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetSeekerSkillById/{id:int}")]

        public async Task<ActionResult<SeekerSkill>> GetSeekerSkillById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddSeekerSkill")]

        public async Task<IActionResult> AddSeekerSkill(SeekerSkill seekerSkill)
        {
            await _service.Create(seekerSkill);
            return Ok($"Sucessfull added new seeker skill: {seekerSkill.skill_name}");
        }

        [HttpPut]
        [Route("UpdateSeekerSkill/{id:int}")]

        public async Task<IActionResult> UpdateSeekerSkill(SeekerSkill reqSeekerSkill, int id)
        {
            if (id != reqSeekerSkill.skill_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(reqSeekerSkill);
            return Ok($"Sucessfully Updated: Seeker skill {reqSeekerSkill.skill_name}");
        }

        [HttpDelete]
        [Route("DeleteSeekerSkillById/{id:int}")]

        public async Task<IActionResult> DeleteSeekerSkillById(int id)
        {
            await _service.Delete(id);
            return Ok($"Suncessfully Deleted: seeker skill {id}");
        } 
    }
}
