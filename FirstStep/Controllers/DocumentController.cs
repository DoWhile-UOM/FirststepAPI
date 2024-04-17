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
        [Route("{eTag}")]
        public async Task<IActionResult> GetDocumentByETag(string eTag)
        {
            if (string.IsNullOrEmpty(eTag))
            {
                return BadRequest("Please provide a valid ETag");
            }

            var blobClient = await _azureBlobService.(eTag);
            if (blobClient == null)
            {
                return NotFound("No document found with the provided ETag");
            }

            // Download the file content (optional)
            var downloadContent = await blobClient.DownloadStreamingAsync();

            // Prepare the file for download based on content type (optional)
            if (downloadContent.ContentType != null)
            {
                return File(downloadContent, downloadContent.ContentType, blobClient.Name);
            }
            else
            {
                // Handle scenario where content type is unknown (optional)
                return Content("Document found, but content type unavailable.", "text/plain");
            }
        }

    }
}
