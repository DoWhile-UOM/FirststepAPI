using FirstStep.Data;
using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllCompanies")]

        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetCompanyById/{companyId:int}")]

        public async Task<ActionResult<Company>> GetCompanyById(int companyId)
        {
            return Ok(await _service.GetById(companyId));
        }

        [HttpPost]
        [Route("AddCompany")]

        public async Task<IActionResult> AddCompany(CompanyDto newCompany)
        {
            await _service.Create(newCompany);
            return Ok($"Successfully added new unregistered company: {newCompany.company_name}.");
        }

        [HttpPut]
        [Route("UpdateCompany/{companyId:int}")]

        public async Task<IActionResult> UpdateCompany(Company reqCompany, int companyId)
        {
            if (companyId != reqCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.Update(companyId, reqCompany);
            return Ok($"Successfully updated unregistered company: {reqCompany.company_name}.");
        }

        [HttpDelete]
        [Route("DeleteCompany/{companyId:int}")]

        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            await _service.Delete(companyId);
            return Ok($"Successfully removed unregistered company: {companyId}.");
        }
    }
}
