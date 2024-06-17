using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly ISystemAdminService _service;

        public SystemAdminController(ISystemAdminService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllSystemAdmins")]

        public async Task<ActionResult<IEnumerable<SystemAdmin>>> GetAllSystemAdmins()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetSystemAdminById/{id}")]

        public async Task<ActionResult<SystemAdmin>> GetSystemAdminById(int id)
        {
            return Ok(await _service.GetById(id));
        }
        [HttpGet]
        [Route("GetLoggingsDetails")]
        public async Task<ActionResult<LoggingsDto>> GetLoggings()
        {
            var loggingsDto = await _service.GetLoggingsOfUsersAsync();
            return Ok(loggingsDto);
        }

        [HttpPost]
        [Route("AddSystemAdmin")]

        public async Task<IActionResult> AddSystemAdmin(SystemAdmin systemAdmin)
        {
            await _service.Create(systemAdmin);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateSystemAdmin")]

        public async Task<IActionResult> UpdateSystemAdmin(SystemAdmin reqsystemAdmin)
        {
            await _service.Update(reqsystemAdmin);
            return Ok($"Successfully Updated SystemAdminID: {reqsystemAdmin.user_id}");
        }
        [HttpDelete]
        [Route("DeleteSystemAdmin/{id}")]

        public async Task<IActionResult> DeleteSystemAdmin(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
