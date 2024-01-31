using FirstStep.Models;
using FirstStep.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredCompanyController : ControllerBase
    {
        private readonly IRegisteredCompanyService _service;

        public RegisteredCompanyController(IRegisteredCompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllRegisteredCompanies")]

        public async Task<ActionResult<IEnumerable<RegisteredCompany>>> GetAllRegisteredCompanies()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetRegisteredCompanyById/{id}")]

        public async Task<ActionResult<RegisteredCompany>> GetRegisteredCompanyById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        /*

        [HttpPost]
        [Route("AddRegisteredCompany")]

        public async Task<ActionResult<RegisteredCompany>> AddRegisteredCompany(RegisteredCompany registeredCompany)
        {
            /*
            try
            {
                var createdRegisteredCompany = await _service.Create(registeredCompany);
                return Ok(createdRegisteredCompany);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }*
        }
    */

        [HttpPut]
        [Route("UpdateRegisteredCompany")]

        public async Task<IActionResult> UpdateRegisteredCompany(RegisteredCompany reqRegisteredCompany)
        {
            await _service.Update(reqRegisteredCompany);
            return Ok("Successfully Updated Company Details");
        }

        [HttpDelete]
        [Route("DeleteRegisteredCompany/{id}")]

        public async Task<IActionResult> DeleteRegisteredCompany(int id)
        {
            await _service.Delete(id);
            return Ok("Successfully Deleted Company");
        }
    }
}
