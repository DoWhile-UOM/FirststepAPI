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
        [Route("GetCompanyById{id}")]

        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("AddCompany")]

        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            return Ok(await _service.Create(company));
        }

        [HttpPut]
        [Route("UpdateCompany")]

        public IActionResult UpdateCompany(Company reqCompany)
        {
            _service.Update(reqCompany);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCompany{id}")]
        public IActionResult DeleteCompany(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
