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
        public async Task<ActionResult<IEnumerable<RegisteredCompany>>> GetRegisteredCompanies()
        {
            try
            {
                var registeredCompanies = await _service.GetAll();
                return Ok(registeredCompanies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRegisteredCompanyById/{id}")]
        public async Task<ActionResult<RegisteredCompany>> GetRegisteredCompanyById(int id)
        {
            try
            {
                var registeredCompany = await _service.GetById(id);
                return Ok(registeredCompany);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddRegisteredCompany")]
        public async Task<ActionResult<RegisteredCompany>> AddRegisteredCompany(RegisteredCompany registeredCompany)
        {
            try
            {
                var createdRegisteredCompany = await _service.Create(registeredCompany);
                return Ok(createdRegisteredCompany);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRegisteredCompany")]
        public IActionResult UpdateRegisteredCompany(RegisteredCompany reqRegisteredCompany)
        {
            try
            {
                _service.Update(reqRegisteredCompany);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteRegisteredCompany/{id}")]
        public IActionResult DeleteRegisteredCompany(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
