
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FirstStep.Services

{
     public class AzureBlobService : IAzureBlobService
    {
          BlobServiceClient _blobServiceClient;
          BlobContainerClient _blobcontainerClient;
          string azureconnectionstring = "DefaultEndpointsProtocol=https;AccountName=firststepstore;AccountKey=1eWRQrfo08HFx8T+eqk4Ja4M4kjZ2zRdfPPrbINpF4XbFbBG5pOCg4qI5aI5sPq0qI/13CCNJbY4+ASthzeFbw==;EndpointSuffix=core.windows.net";

          public AzureBlobService()

          {
            
            _blobServiceClient = new BlobServiceClient(azureconnectionstring);
            _blobcontainerClient = _blobServiceClient.GetBlobContainerClient("apiimages");
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
      }
  }
   
  
    



