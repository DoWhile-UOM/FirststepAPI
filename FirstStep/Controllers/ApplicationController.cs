using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Firststep.Exception.cs;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllApplications")]
        public async Task<ActionResult<IEnumerable<Application>>>GetAllApplications()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet]
        [Route("GetApplicationById/{id}")]
        public async Task<ActionResult<Application>> GetApplicationById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpGet]
        [Route("GetHRManagerApplicationListByAdvertisementID/JobID={jobId:int}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetHRManagerApplicationList(int jobId)
        {
            return Ok(await _service.GetHRManagerAdertisementListByJobID(jobId));
        }

        [HttpGet]
        [Route("GetApplicationsBySeekerId/{id}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsBySeekerId(int id)
        {
            return Ok(await _service.GetBySeekerId(id));
        }

        [HttpPost]
        [Route("AddApplication")]
        public async Task<IActionResult> AddApplication(AddApplicationDto newApplication)
        {
            await _service.Create(newApplication);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateApplication")]
        public async Task<IActionResult> UpdateCApplication(Application reqApplication)
        {
            await _service.Update(reqApplication);            
            return Ok($"Successfully Updated Application ID: {reqApplication.application_Id}");
        }

        [HttpDelete]
        [Route("DeleteApplication/{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
        //task delegation
        [HttpPost]
        [Route("DelegateTask/{companyId,advertisement}")]
        public async Task InitiateTaskDelegation(int company_id, Advertisement advertisement)
        {
            try
            {
                return Ok(await _service.InitiateTaskDelegation(company_id, advertisement));
            } 
            catch(Exception e)
            {
                return ReturnStatusCode(e);
            }
        }
    }
}
