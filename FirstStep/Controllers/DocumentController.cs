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

        [HttpGet("{eTag}")]
        public async Task<IActionResult> GetBlobByETag(string eTag)
        {
            var blob = await _azureBlobService.GetBlobByETag(eTag);
            if (blob == null)
            {
                return NotFound();
            }
            return Ok(blob);
        }

        [HttpGet("download/{eTag}")]
        public async Task<IActionResult> DownloadBlobByETag(string eTag)
        {
            var blob = await _azureBlobService.GetBlobByETag(eTag);
            if (blob == null)
            {
                return NotFound();
            }
            var file = await _azureBlobService.DownloadBlobByETag(eTag);
            return File(file, "application/octet-stream", blob.Name);
        }   
    }
}
