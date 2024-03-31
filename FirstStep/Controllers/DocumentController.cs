using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        public IAzureBlobService _azureBlobService;

        public DocumentController(IAzureBlobService service)
        {
            _azureBlobService = service;
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlobs(List<IFormFile> files)
        {
            var response = await _azureBlobService.UploadFiles(files);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlobs()
        {
            var response = await _azureBlobService.GetUploadedBlobs();
            return Ok(response);
        }

        [HttpGet]
        [Route("DownloadBlob/{eTag}")]    
        public async Task<IActionResult> DownloadBlob(string eTag)
        {
            return Ok(await _azureBlobService.DownloadBlob(eTag));
        }

/*
        [HttpDelete]
        public async Task<IActionResult> DeleteBlob(string blobName)
        {
            await _azureBlobService.DeleteBlob(blobName);
            return Ok();
        }*/
    }
}
