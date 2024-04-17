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
        public async Task<ActionResult<AdvertisementFirstPageDto>> GetAdvertisements(int seekerID, int pageLength)
        {
            return Ok(await _service.GetFirstPage(seekerID, pageLength));
        }

        [HttpGet]
        [Route("GetAllAdvertisements/seekerID={seekerID:int}/advertisements={jobIDs}")]
        public async Task<ActionResult<IEnumerable<AdvertisementShortDto>>> GetAdvertisementsById(string jobIDs, int seekerID)
        {
            return Ok(await _service.GetById(jobIDs.Split(',').Select(int.Parse), seekerID));
        }

        [HttpGet]
        [Route("GetAdvertisementById/{jobID:int}")]
        public async Task<ActionResult<AdvertisementDto>> GetAdvertisementById(int jobID)
        {            
            return Ok(await _service.GetById(jobID));
        }

        [HttpGet]
        [Route("GetAdvertisementById/update/{jobID:int}")]
        public async Task<ActionResult<UpdateAdvertisementDto>> GetAdvertisementByIdWithKeywords(int jobID)
        {
            return Ok(await _service.GetByIdWithKeywords(jobID));
        }

        [HttpGet]
        [Route("GetAdvertisementsByCompanyID/{companyID:int}/filterby={status}")]
        public async Task<ActionResult<IEnumerable<AdvertisementTableRowDto>>> GetAdvertisementsByCompanyID(int companyID, string status)
        {
            return Ok(await _service.GetByCompanyID(companyID, status));
        }

        [HttpGet]
        [Route("GetAdvertisementsByCompanyID/{companyID:int}/filterby={status}/title={title}")]
        public async Task<ActionResult<IEnumerable<AdvertisementTableRowDto>>> GetAdvertisementsByCompanyID(int companyID, string status, string title)
        {
            return Ok(await _service.GetByCompanyID(companyID, status, title));
        }

        [HttpGet]
        [Route("GetSavedAdvertisements/seekerID={seekerID:int}")]
        public async Task<ActionResult<IEnumerable<AdvertisementShortDto>>> GetSavedAdvertisements(int seekerID)
        {
            return Ok(await _service.GetSavedAdvertisements(seekerID));
        }

        [HttpPost]
        [Route("SearchAdvertisementsBasic/seekerID={seekerID:int}/pageLength={pageLength:int}")]
        public async Task<ActionResult<AdvertisementFirstPageDto>> SearchAdvertisementsBasic(int seekerID, int pageLength, SearchJobRequestDto requestDto)
        {
            return Ok(await _service.BasicSearch(requestDto, seekerID, pageLength));
        }

        [HttpPost]
        [Route("SearchAdvertisementsAdvance/seekerID={seekerID:int}")]
        public async Task<ActionResult<IEnumerable<AdvertisementShortDto>>> SearchAdvertisementsAdvanced(int seekerID, SearchJobRequestDto requestDto)
        {
            return Ok(await _service.AdvanceSearch(requestDto, seekerID));
        }

        [HttpPost]
        [Route("AddAdvertisement")]
        public async Task<IActionResult> AddAdvertisement(AddAdvertisementDto advertisementDto)
        {
            await _service.Create(advertisementDto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateAdvertisement/{jobID:int}")]
        public async Task<IActionResult> UpdateAdvertisement(UpdateAdvertisementDto reqAdvertisement, int jobID)
        {
            if (reqAdvertisement is null)
            {
                return BadRequest("Advertisement cannot be null.");
            }

            if (jobID != reqAdvertisement.advertisement_id)
            {
                return BadRequest("Advertisement ID mismatch.");
            }

            await _service.Update(jobID, reqAdvertisement);
            return Ok();
        }

        [HttpPut]
        [Route("ChangeStatus/{jobID:int}/status={newStatus}")]
        public async Task<IActionResult> ChangeStatus(int jobID, string newStatus)
        {
            await _service.ChangeStatus(jobID, newStatus);
            return Ok();
        }

        [HttpPut]
        [Route("SaveAdvertisement/{jobID:int}/save={isSave:bool}/seekerId={seekerId:int}")]
        public async Task<IActionResult> SaveAdvertisement(int jobID, int seekerId, bool isSave)
        {
            await _service.SaveAdvertisement(jobID, seekerId, isSave);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/{jobID:int}")]
        public async Task<IActionResult> DeleteAdvertisement(int jobID)
        {
            await _service.Delete(jobID);
            return Ok();
        }
    }
}
