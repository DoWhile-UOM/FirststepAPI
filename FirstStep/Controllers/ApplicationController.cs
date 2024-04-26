using FirstStep.Models;
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



        [HttpGet]
        [Route("TotalEvaluatedApplications/{advertisment_id}")]

        public async Task<ActionResult<int>> TotalEvaluatedApplications(int advertisment_id)
        {
            return Ok(await _service.TotalEvaluatedApplications(advertisment_id));
        }

        [HttpGet]
        [Route("TotalNotEvaluatedApplications/{advertisment_id}")]
        public async Task<ActionResult<int>> TotalNotEvaluatedApplications(int advertisment_id)
        {
            return Ok(await _service.TotalNotEvaluatedApplications(advertisment_id));
        }

        [HttpGet]
        [Route("AcceptedApplications/{advertisment_id}")]
        public async Task<ActionResult<int>> AcceptedApplications(int advertisment_id)
        {
            return Ok(await _service.AcceptedApplications(advertisment_id));
        }

        [HttpGet]
        [Route("RejectedApplications/{advertisment_id}")]
        public async Task<ActionResult<int>> RejectedApplications(int advertisment_id)
        {
            return Ok(await _service.RejectedApplications(advertisment_id));
        }


        [HttpPost]
        [Route("AddApplication")]

        public async Task<IActionResult> AddApplication(Application application)
        {
            await _service.Create(application);
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

    }
}
