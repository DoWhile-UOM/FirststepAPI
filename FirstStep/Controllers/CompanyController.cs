using FirstStep.Data;
using FirstStep.Models;
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
        [Route("GetCompanyById/{id}")]

        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddCompany")]

        public async Task<ActionResult> AddCompany(Company company)
        {
            await _service.Create(company);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateCompany")]

        public async Task<IActionResult> UpdateCompany(Company reqCompany)
        {
            await _service.Update(reqCompany);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
