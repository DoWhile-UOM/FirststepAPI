using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace FirstStep.Services
{
    public class FileService : IFileService
    {
        BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobcontainerClient;

        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _blobcontainerClient = _blobServiceClient.GetBlobContainerClient("firststep");
        }

        public async Task<List<string>> UploadFiles(List<IFormFile> files)
        {

            var fileNames = new List<string>();
            foreach (var file in files)
            {
                string fileName = file.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    await _blobcontainerClient.UploadBlobAsync(fileName, memoryStream, default);
                    fileNames.Add(fileName);
                }
            };

            return fileNames;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _blobcontainerClient.GetBlobClient(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return fileName;
        }

        public async Task<List<BlobItem>> GetUploadedBlobs()
        {
            var items = new List<BlobItem>();
            var uploadedFiles = _blobcontainerClient.GetBlobsAsync();
            await foreach (BlobItem file in uploadedFiles)
            {
                items.Add(file);
            }

            return items;
        }

        public async Task<string> GenerateSasTokenAsync(string blobName)
        {
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobcontainerClient.Name,
                BlobName = blobName,
                Resource = "b",//blob
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(24)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS token
            var sasQueryParamas = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential("firststep", "uufTzzJ+uB7BRnKG9cN2RUi0mw92n5lTl2EMvnOTw6xv7sfPQSWBqJxHll+Zn2FNc06cGf8Qgrkb+ASteH1KEQ==")).ToString();

            var sasToken = sasQueryParamas.ToString();

            return await Task.FromResult(sasToken);

        }

        public async Task<string> GetBlobUrl(string blobName)
        {
            var sasToken = await GenerateSasTokenAsync(blobName);
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);

            //set the content disposition header to inline
            var headers = new BlobHttpHeaders
            {
                ContentDisposition = "inline"
            };
            await blobClient.SetHttpHeadersAsync(headers);

            var blobUrlWithSas = $"{blobClient.Uri}?{sasToken}";
            return blobUrlWithSas;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
 }
