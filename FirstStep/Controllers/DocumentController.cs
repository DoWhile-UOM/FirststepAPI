using FirstStep.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        public IFileService _azureBlobService;

        public DocumentController(IFileService service)
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

        [HttpGet("sas/{blobName}")]
        public async Task<IActionResult> GetSasToken(string blobName)
        {
            var response = await _azureBlobService.GenerateSasTokenAsync(blobName);
            return Ok(response);
        }

        [HttpGet("url/{blobName}")]

        public async Task<IActionResult> GetBlobUrl(string blobName)
        {
            var response = await _azureBlobService.GetBlobImageUrl(blobName);
            return Ok(response);
        }

        [HttpDelete("{blobName}")]
        public async Task<IActionResult> DeleteBlob(string blobName)
        {
            await _azureBlobService.DeleteBlobAsync(blobName);
            return Ok();
        }
    }
}
