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

        [HttpGet]
        [Route("GetAllBlobs")]
        public async Task<IActionResult> GetAllBlobs()
        {
            var response = await _azureBlobService.GetUploadedBlobs();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSasToken")]
        public async Task<IActionResult> GetSasToken(string blobName)
        {
            var response = await _azureBlobService.GenerateSasTokenAsync(blobName);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetBlobUrl")]
        public async Task<IActionResult> GetBlobUrl(string blobName)
        {
            var response = await _azureBlobService.GetBlobImageUrl(blobName);
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteBlob/{blobName}")]
        public async Task<IActionResult> DeleteBlob(string blobName)
        {
            await _azureBlobService.DeleteBlobAsync(blobName);
            return Ok();
        }
    }
}
