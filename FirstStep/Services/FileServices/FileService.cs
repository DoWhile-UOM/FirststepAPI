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
        StorageSharedKeyCredential _storageSharedKeyCredential;

        public FileService(BlobServiceClient blobServiceClient, StorageSharedKeyCredential storageSharedKeyCredential)
        {
            _storageSharedKeyCredential = storageSharedKeyCredential;
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

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while uploading the file.", ex);
            }
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
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobcontainerClient.Name,
                BlobName = blobName,
                Resource = "b",//blob
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS token
            var sasQueryParamas = sasBuilder.ToSasQueryParameters(_storageSharedKeyCredential).ToString();

            var sasToken = sasQueryParamas.ToString();

            return await Task.FromResult(sasToken);
        }

        public async Task<string> GetBlobUrl(string blobName)
        {
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);

            // Check if the blob exists
            bool exists = await blobClient.ExistsAsync();
            if (!exists)
            {
                return "";
            }

            var sasToken = await GenerateSasTokenAsync(blobName);

            // Set the content disposition header to inline
            var headers = new BlobHttpHeaders
            {
                ContentDisposition = "inline"
            };
            await blobClient.SetHttpHeadersAsync(headers);

            var blobUrlWithSas = $"{blobClient.Uri}?{sasToken}";
            return blobUrlWithSas;
        }

        public async Task DeleteBlob(string blobName)
        {
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
 }
