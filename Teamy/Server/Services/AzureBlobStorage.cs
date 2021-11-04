using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Teamy.Server.Services
{
    public class BlobStorageOptions
    {
        public string ConnectionString { get; set; }
        public string ImagesContainer { get; set; }
    }

    public interface IStorageService
    {
        Task<Uri> Upload(Stream fileStream, string fileName);
        Task<bool> Delete(string filename);
    }

    public class AzureBlobStorageService : IStorageService
    {
        BlobStorageOptions _options;
        public AzureBlobStorageService(IOptions<BlobStorageOptions> options)
        {
            _options = options.Value;
        }

        public async Task<Uri> Upload(Stream fileStream, string fileName)
        {
            BlobServiceClient blobServiceClient = new(_options.ConnectionString);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_options.ImagesContainer); 
            if(!container.Exists())
            {
                await blobServiceClient.CreateBlobContainerAsync(_options.ImagesContainer, PublicAccessType.BlobContainer);
            }

            BlobClient blobClient = new(_options.ConnectionString, _options.ImagesContainer, fileName);

            await blobClient.UploadAsync(fileStream);
            return await Task.FromResult(blobClient.Uri);
        }

        public async Task<bool> Delete(string fileName)
        {
            BlobClient blobClient = new(_options.ConnectionString, _options.ImagesContainer, fileName);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
            return true;
        }
    }
}
