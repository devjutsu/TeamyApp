using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Teamy.Server.Services
{
    public interface IStorageService
    {
        Task<Uri> Upload(Stream fileStream, string fileName);
        Task<bool> Delete(string filename);
    }

    public class AzureBlobStorageService : IStorageService
    {
        readonly string connectionString;
        readonly string containerName;
        public AzureBlobStorageService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureImageStore");
            this.containerName = configuration.GetSection("Storage:ImagesContainer").Value;
        }

        public async Task<Uri> Upload(Stream fileStream, string fileName)
        {
            BlobServiceClient blobServiceClient = new(connectionString);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName); 
            if(!container.Exists())
            {
                await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.BlobContainer);
            }

            BlobClient blobClient = new(connectionString, containerName, fileName);

            await blobClient.UploadAsync(fileStream);
            return await Task.FromResult(blobClient.Uri);
        }

        public async Task<bool> Delete(string fileName)
        {
            BlobClient blobClient = new(connectionString, containerName, fileName);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
            return true;
        }
    }
}
