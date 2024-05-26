using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
        [Route("GetApplicationList/JobID={jobId:int}/status={status}")]
        public async Task<ActionResult<ApplicationListingPageDto>> GetApplicationList(int jobId, string status)
        {
            return Ok(await _service.GetApplicationList(jobId, status));
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
        [Route("DelegateTask/{company_id}/{advertisement_id}")]
        public async Task<IActionResult> DelegateTaskToHRAssistants(int company_id, int advertisement_id)
        {
            if (company_id == 0|| advertisement_id==0)
            {
                Console.WriteLine("0");
                return NoContent();
            }
            try
             {
                // Check whether that advertisementID exists and initiate task delegation
                Console.WriteLine("1");
                await _service.InitiateTaskDelegation(company_id, advertisement_id);
                 return Ok("Task delegation initiated successfully.");
             }
             catch (NullReferenceException ex) when (ex.Message == "No applications for evaluation.")
             {
                Console.WriteLine("2");
                return NoContent(); // HTTP 204 No Content
             }
             catch (NullReferenceException ex) when (ex.Message == "Not enough HR Assistants for task delegation.")
             {
                Console.WriteLine("3");
                return BadRequest("Not enough HR Assistants for task delegation."); // HTTP 400 Bad Request
             }
             catch (Exception ex)
             {
                Console.WriteLine("4");
                return StatusCode(500, $"Internal server error: {ex.Message}"); // HTTP 500 Internal Server Error
             }
           
              
        }


    }
}
