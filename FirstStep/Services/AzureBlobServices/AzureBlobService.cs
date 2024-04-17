
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FirstStep.Services

{
    public class AzureBlobService : IAzureBlobService
    {
        BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobcontainerClient;
        string azureconnectionstring = "DefaultEndpointsProtocol=https;AccountName=firststep;AccountKey=uufTzzJ+uB7BRnKG9cN2RUi0mw92n5lTl2EMvnOTw6xv7sfPQSWBqJxHll+Zn2FNc06cGf8Qgrkb+ASteH1KEQ==;EndpointSuffix=core.windows.net";

        public AzureBlobService()
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

        public async Task<BlobItem> GetBlobByETag(string eTag)
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
    }
}
