using Azure.Storage.Blobs.Models;

namespace FirstStep.Services
{
    public interface IAzureBlobService
    {
        public Task<List<Azure.Response<BlobContentInfo>>> UploadFiles(List<IFormFile> files);

        public Task<List<BlobItem>> GetUploadedBlobs();
<<<<<<< HEAD

        public Task<Stream> DownloadBlob(string eTag);


=======
>>>>>>> parent of 4f4ab32 (failed)
    }
}
