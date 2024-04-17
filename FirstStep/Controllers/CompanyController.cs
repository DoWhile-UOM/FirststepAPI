using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FirstStep.Services.CompanyService;

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
        public async Task<ActionResult<CompanyProfileDetailsDto>> GetCompanyById(int companyId)
        {
            return Ok(await _service.GetById(companyId));
        }

        [HttpGet]
        [Route("GetAllUnregisteredCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetUnregisteredCompanies()
        {
            return Ok(await _service.GetAllUnregisteredCompanies());
        }

        [HttpGet]
        [Route("GetAllRegisteredCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetRegisteredCompanies()
        {
            return Ok(await _service.GetAllRegisteredCompanies());
        }

        [HttpGet]
        [Route("GetCompanyProfile/{companyID:int}/seekerID={seekerID:int}/pageLength={pageLength:int}")]
        public async Task<ActionResult<CompanyProfileDto>> GetCompanyProfile(int companyID, int seekerID, int pageLength)
        {
            return Ok(await _service.GetCompanyProfile(companyID, seekerID, pageLength));
        }

        [HttpPost]
        [Route("AddCompany")] // Company Admin
        public async Task<IActionResult> AddCompany(AddCompanyDto newCompany)
        {
            try
            {
                await _service.Create(newCompany);
                return Ok("Company Application successfully filled!");
            }
            catch (EmailAlreadyExistsException ex)//Handle Email Already Exists Exception
            {
                return BadRequest(ex.Message);
            }
            catch (RegistrationNumberAlreadyExistsException ex)// Handle Registration Number Already Exists Exception
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)//Handle other errors
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut]
        [Route("UpdateCompanyVerification/{companyId:int}")] // System Admin
        public async Task<IActionResult> UpdateCompany(CompanyRegInfoDto reqCompany, int companyId)
        {
            if (companyId != reqCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.UpdateCompanyVerification(companyId, reqCompany);
            return Ok($"Successfully updated verification details of unregistered company on ID: {reqCompany.company_id}.");
        }

        [HttpPut]
        [Route("RegisterCompany/{companyId:int}")] // Company Admin
        public async Task<IActionResult> RegisterCompany(AddDetailsCompanyDto reqCompany, int companyId)
        {
            if (companyId != reqCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.RegisterCompany(companyId, reqCompany);
            return Ok($"Successfully registered company on ID: {companyId}.");
        }

        [HttpPut]
        [Route("UpdateUnregisteredCompany/{companyId:int}")] // Company Admin
        public async Task<IActionResult> UpdateUnregisteredCompany(UpdateUnRegCompanyDto reqCompany, int companyId)
        {
            if (companyId != reqCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.UpdateUnregisteredCompany(companyId, reqCompany);
            return Ok($"Successfully updated unregistered company on ID: {companyId}.");
        }

        [HttpPut]
        [Route("UpdateRegisteredCompany/{companyId:int}")] // Company Admin
        public async Task<IActionResult> UpdateRegisteredCompany(UpdateCompanyDto reqCompany, int companyId)
        {
            if (companyId != reqCompany.company_id)
            {
                return BadRequest("Context is not matching");
            }

            await _service.UpdateRegisteredCompany(companyId, reqCompany);
            return Ok($"Successfully updated registered company on ID: {companyId}.");
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
