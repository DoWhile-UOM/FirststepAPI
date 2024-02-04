using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
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
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetRegisteredCompanyById/{id:int}")]
        public async Task<ActionResult<RegisteredCompany>> GetRegisteredCompanyById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPut]
        [Route("UpdateRegisteredCompany/{id:int}")]
        public async Task<IActionResult> UpdateRegisteredCompany(RegisteredCompany reqRegisteredCompany, int id)
        {
            if (id != reqRegisteredCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(reqRegisteredCompany);
            return Ok($"Sucessfully Updated: Registered company {reqRegisteredCompany.company_name}");
        }

        [HttpDelete]
        [Route("DeleteRegisteredCompanyById/{id:int}")]
        public async Task<IActionResult> DeleteRegisteredCompanyById(int id)
        {
            await _service.Delete(id);
            return Ok($"Suncessfully Deleted: registered company {id}");
        }

        [HttpPost]
        [Route("SetAsRegisteredCompany/{companyID:int}")]
        public async Task<IActionResult> SetAsRegisteredCompany(RegisteredCompanyDto newRegCompany,int companyID)
        {
            await _service.SetAsRegistered(companyID, newRegCompany);
            return Ok($"Sucessfully Set: Registered company {companyID}");
        }
    }
}

