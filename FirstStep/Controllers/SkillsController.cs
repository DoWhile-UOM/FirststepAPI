using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _service;

        public SkillsController(ISkillService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllSkills")]

        public async Task<ActionResult<IEnumerable<string>>> GetSkills()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetSkillById/{id:int}")]

        public async Task<ActionResult<Skill>> GetSkillById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddSkill/{skillName}")]

        public async Task<IActionResult> AddSkill(string skillName)
        {
            await _service.Create(skillName);
            return Ok($"Sucessfull added new seeker skill: {skillName}");
        }

        [HttpPut]
        [Route("UpdateSkill/{id:int}")]

        public async Task<IActionResult> UpdateSkill(Skill reqSkill, int id)
        {
            if (id != reqSkill.skill_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(id, reqSkill);
            return Ok($"Sucessfully Updated: Seeker skill {reqSkill.skill_name}");
        }

        [HttpDelete]
        [Route("DeleteSkillById/{id:int}")]

        public async Task<IActionResult> DeleteSkillById(int id)
        {
            await _service.Delete(id);
            return Ok($"Suncessfully Deleted: seeker skill {id}");
        } 
    }
}
