using Azure.Storage.Blobs.Models;

namespace FirstStep.Services
{
    public interface IFileService
    {
        public Task<List<Azure.Response<BlobContentInfo>>> UploadFiles(List<IFormFile> files);

        public Task<List<BlobItem>> GetUploadedBlobs();

        Task<BlobItem?> GetBlobByETag(string eTag);

        Task<byte[]>? DownloadBlobByETag(string eTag);
    }
}
