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
        string azureconnectionstring = "DefaultEndpointsProtocol=https;AccountName=firststep;AccountKey=uufTzzJ+uB7BRnKG9cN2RUi0mw92n5lTl2EMvnOTw6xv7sfPQSWBqJxHll+Zn2FNc06cGf8Qgrkb+ASteH1KEQ==;EndpointSuffix=core.windows.net";

        public FileService()
        {
            _blobServiceClient = new BlobServiceClient(azureconnectionstring);
            _blobcontainerClient = _blobServiceClient.GetBlobContainerClient("firststep");
        }
        
        public async Task<List<Azure.Response<BlobContentInfo>>> UploadFiles(List<IFormFile> files)
        {

            var azureResponse = new List<Azure.Response<BlobContentInfo>>();
            foreach (var file in files)
            {
                string fileName = file.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    var client = await _blobcontainerClient.UploadBlobAsync(fileName, memoryStream, default);
                    azureResponse.Add(client);
                }
            };

            return azureResponse;
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

        public async  Task<string> GenerateSasTokenAsync( string blobName)
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

            var sasToken= sasQueryParamas.ToString();

            return await Task.FromResult(sasToken);

        }

        public async Task<string> GetBlobImageUrl(string blobName)
        {
            var sasToken = await GenerateSasTokenAsync(blobName);
            var blobClient = _blobcontainerClient.GetBlobClient(blobName);

            var blobUrlWithSas = $"{blobClient.Uri}?{sasToken}";
            return blobUrlWithSas;
        }





        /* public async Task<BlobItem?> GetBlobByETag(string eTag)
         {
             await foreach (BlobItem blob in _blobcontainerClient.GetBlobsAsync())
             {
                 var properties = await _blobcontainerClient.GetBlobClient(blob.Name).GetPropertiesAsync();
                 if (properties.Value.ETag.ToString() == eTag)
                 {
                     return blob;
                 }

             }
             return null;

         }

         public Task<byte[]>? DownloadBlobByETag(string eTag)
         {

             var blobItem = GetBlobByETag(eTag).Result;
             if (blobItem != null)
             {
                 var blobClient = _blobcontainerClient.GetBlobClient(blobItem.Name);
                 var download = blobClient.DownloadContent();
                 return Task.FromResult(download.Value.Content.ToArray());
             }
             return null;
         }*/



    }
}
