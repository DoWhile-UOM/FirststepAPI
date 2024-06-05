using FirstStep.Models;
using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Route("GetAssignedApplicationList/hraId={hraId:int}/JobID={jobId:int}/status={status}")]
        public async Task<ActionResult<ApplicationListingPageDto>> GetAssignedApplicationList(int hraId, int jobId, string status)
        {
            return Ok(await _service.GetAssignedApplicationList(hraId, jobId, status));
        }

        [HttpGet]
        [Route("GetApplicationsBySeekerId/{id}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsBySeekerId(int id)
        {
            return Ok(await _service.GetBySeekerId(id));
        }

        //get appplication status by application ID
        [HttpGet]
        [Route("GetApplicationStatus/{id}")]
        public async Task<ActionResult<ApplicationStatusDto>> GetApplicationStatus(int id)
        {
            return Ok(await _service.GetApplicationStatus(id));
        }






        [HttpPost]
        [Route("AddApplication")]
        public async Task<IActionResult> AddApplication([FromForm] AddApplicationDto newApplication)
        {
            await _service.Create(newApplication);
            return Ok();
        }

        [HttpPatch]
        [Route("DelegateTask/jobID={jobID}")]
        public async Task<IActionResult> DelegateTaskToHRAssistants(int jobID)
        {
            try
            {
                await _service.InitiateTaskDelegation(jobID, null);
                return Ok("Task delegation initiated successfully.");
            }
            catch (NullReferenceException ex) when (ex.Message == "No applications for evaluation.")
            {
                return NoContent(); // HTTP 204 No Content
            }
            catch (NullReferenceException ex) when (ex.Message == "Not enough HR Assistants for task delegation.")
            {
                return BadRequest("Not enough HR Assistants for task delegation."); // HTTP 400 Bad Request
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // HTTP 500 Internal Server Error
            }
        }

        [HttpPatch]
        [Route("DelegateTask/jobID={jobID}/hra_id_list={hra_id_list}")]
        public async Task<IActionResult> DelegateTaskToHRAssistants(int jobID, string hra_id_list)
        {
            try
            {
                await _service.InitiateTaskDelegation(jobID, hra_id_list.Split(',').Select(int.Parse));
                return Ok("Task delegation initiated successfully.");
            }
            catch (NullReferenceException ex) when (ex.Message == "No applications for evaluation.")
            {
                return NoContent(); // HTTP 204 No Content
            }
            catch (NullReferenceException ex) when (ex.Message == "Not enough HR Assistants for task delegation.")
            {
                return BadRequest("Not enough HR Assistants for task delegation."); // HTTP 400 Bad Request
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // HTTP 500 Internal Server Error
            }
        }

        [HttpPatch]
        [Route("ChangeAssignedHRA/applicationId={applicationId}/hraId={hraId}")]
        public async Task<IActionResult> ChangeAssignedHRA(int applicationId, int hraId)
        {
            await _service.ChangeAssignedHRA(applicationId, hraId);
            return Ok("Successfully changed assigned HRA.");
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
    }
}
