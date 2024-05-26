using FirstStep.Models.DTOs;
using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _service;

        public AdvertisementController(IAdvertisementService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllAdvertisements/seekerID={seekerID:int}/pageLength={pageLength:int}")]
        public async Task<ActionResult<AdvertisementFirstPageDto>> GetAllAdvertisements(int seekerID, int pageLength)
        {
            try
            {
                return Ok(await _service.GetAllWithPages(seekerID, pageLength));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAllAdvertisements/seekerID={seekerID:int}/advertisements={jobIDs}")]
        public async Task<ActionResult<IEnumerable<AdvertisementShortDto>>> GetAdvertisementsById(string jobIDs, int seekerID)
        {
            try
            {
                return Ok(await _service.GetById(jobIDs.Split(',').Select(int.Parse), seekerID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetRecommendedAdvertisements/seekerID={seekerID:int}/pageLength={pageLength:int}")]
        public async Task<ActionResult<AdvertisementFirstPageDto>> GetRecommendedAdvertisements(int seekerID, int pageLength)
        {
            try
            {
                return Ok(await _service.GetRecommendedAdvertisements(seekerID, pageLength));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetRecommendedAdvertisements/seekerID={seekerID:int}/pageLength={pageLength:int}/city={city}")]
        public async Task<ActionResult<AdvertisementFirstPageDto>> GetRecommendedAdvertisements(int seekerID, string city, int pageLength)
        {
            try
            {
                return Ok(await _service.GetRecommendedAdvertisements(seekerID, city, pageLength));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAdvertisementById/{jobID:int}")]
        public async Task<ActionResult<AdvertisementDto>> GetAdvertisementById(int jobID)
        {
            try
            {
                return Ok(await _service.GetById(jobID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAdvertisementById/update/{jobID:int}")]
        public async Task<ActionResult<UpdateAdvertisementDto>> GetAdvertisementByIdWithKeywords(int jobID)
        {
            try
            {
                return Ok(await _service.GetByIdWithKeywords(jobID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAdvertisementsByCompanyID/{companyID:int}/filterby={status}")]
        public async Task<ActionResult<IEnumerable<AdvertisementTableRowDto>>> GetAdvertisementsByCompanyID(int companyID, string status)
        {
            try
            {
                return Ok(await _service.GetByCompanyID(companyID, status));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAdvertisementsByCompanyID/{companyID:int}/filterby={status}/title={title}")]
        public async Task<ActionResult<IEnumerable<AdvertisementTableRowDto>>> GetAdvertisementsByCompanyID(int companyID, string status, string title)
        {
            try
            {
                return Ok(await _service.GetByCompanyID(companyID, status, title));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAssignedAdvertisementsByHRA/hra_id={hra_userID:int}")]
        public async Task<ActionResult<IEnumerable<AdvertisementTableRowDto>>> GetAssignedAdvertisementsByHRA(int hra_userID)
        {
            try
            {
                return Ok(await _service.GetAssignedAdvertisementsByHRA(hra_userID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetSavedAdvertisements/seekerID={seekerID:int}")]
        public async Task<ActionResult<IEnumerable<AdvertisementShortDto>>> GetSavedAdvertisements(int seekerID)
        {
            try
            {
                return Ok(await _service.GetSavedAdvertisements(seekerID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpGet]
        [Route("GetAppliedAdvertisements/seekerID={seekerID:int}")]
        public async Task<ActionResult<IEnumerable<AppliedAdvertisementShortDto>>> GetAppliedAdvertisements(int seekerID)
        {
            try
            {
                return Ok(await _service.GetAppliedAdvertisements(seekerID));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPost]
        [Route("SearchAdvertisementsBasic/seekerID={seekerID:int}/pageLength={pageLength:int}")]
        public async Task<ActionResult<AdvertisementFirstPageDto>> SearchAdvertisements(int seekerID, int pageLength, SearchJobRequestDto requestDto)
        {
            try
            {
                return Ok(await _service.BasicSearch(requestDto, seekerID, pageLength));
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPost]
        [Route("AddAdvertisement")]
        public async Task<IActionResult> AddAdvertisement(AddAdvertisementDto advertisementDto)
        {
            try
            {
                await _service.Create(advertisementDto);
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPut]
        [Route("UpdateAdvertisement/{jobID:int}")]
        public async Task<IActionResult> UpdateAdvertisement(UpdateAdvertisementDto reqAdvertisement, int jobID)
        {
            if (jobID != reqAdvertisement.advertisement_id)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Advertisement ID in the request body does not match the ID in the URL.");
            }

            try
            {
                await _service.Update(jobID, reqAdvertisement);
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPatch]
        [Route("ChangeStatus/{jobID:int}/status={newStatus}")]
        public async Task<IActionResult> ChangeStatus(int jobID, string newStatus)
        {
            try
            {
                await _service.ChangeStatus(jobID, newStatus);
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPatch]
        [Route("ChangeStatus/{jobID:int}/reactivate/newSubmissionDeadline={newDeadline}")]
        public async Task<IActionResult> ReactivateAdvertisement(int jobID, string newDeadline)
        {
            try
            {
                if (newDeadline == "-1")
                {
                    await _service.ReactivateAdvertisement(jobID, null);
                }
                else
                {
                    await _service.ReactivateAdvertisement(jobID, DateTime.Parse(newDeadline));
                }
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpPatch]
        [Route("SaveAdvertisement/{jobID:int}/save={isSave:bool}/seekerId={seekerId:int}")]
        public async Task<IActionResult> SaveAdvertisement(int jobID, int seekerId, bool isSave)
        {
            if (await _service.IsExpired(jobID))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Advertisement is expired.");
            }

            try
            {
                await _service.SaveAdvertisement(jobID, seekerId, isSave);
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/{jobID:int}")]
        public async Task<IActionResult> DeleteAdvertisement(int jobID)
        {
            try
            {
                await _service.Delete(jobID, false);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, e.Message);
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/confirm=true/{jobID:int}")]
        public async Task<IActionResult> DeleteAdvertisementWithConfirmation(int jobID)
        {
            try
            {
                await _service.Delete(jobID, true);
                return Ok();
            }
            catch (Exception e)
            {
                return ReturnStatusCode(e);
            }
        }

        private ActionResult ReturnStatusCode(Exception e)
        {
            if (e is InvalidDataException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            else if (e is NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
