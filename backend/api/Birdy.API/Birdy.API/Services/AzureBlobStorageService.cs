using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Birdy.API.Services.Interfaces;

namespace Birdy.API.Services
{
    public class AzureBlobStorageService: IAzureBlobStorageService
    {
        private readonly string _blobStorageConnectionKey;
        private readonly string _containerName;

        public AzureBlobStorageService(string blobStorageConnectionKey, string containerName)
        {
            this._blobStorageConnectionKey = blobStorageConnectionKey;
            this._containerName = containerName;
        }

        private async Task<BlobContainerClient> GetBlobContainer()
        {
            // Create blob client and return reference to the container
            BlobContainerClient container = new(_blobStorageConnectionKey, _containerName);

            await container.CreateIfNotExistsAsync();

            return container;
        }

        public async Task<Uri> Upload(string storageFileName, string fileType, byte[] fileStream)
        {
            BlobClient blob;
            try
            {
                var container = await GetBlobContainer();

                blob = container.GetBlobClient(storageFileName);

                using (var stream = new MemoryStream(fileStream))
                {
                    await blob.UploadAsync(stream, overwrite: true);
                }

                await blob.SetHttpHeadersAsync(new BlobHttpHeaders
                {
                    ContentType = fileType
                });

                return blob.Uri;

            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            finally
            {
                fileStream = null;
                blob = null;
            }
        }

        public async Task<(byte[] fileData, string mimeType)> Retrieve(string storageFileName)
        {
            var container = await GetBlobContainer();

            var blob = container.GetBlobClient(storageFileName);

            if (await blob.ExistsAsync())
                return await GetMedia(blob);

            return (null, null);
        }

        private async Task<(byte[] fileData, string mimeType)> GetMedia(BlobClient blobClient)
        {
            using (var stream = new MemoryStream())
            {
                var props = await blobClient.GetPropertiesAsync();
                await blobClient.DownloadToAsync(stream);
                return (fileData: stream.ToArray(), mimeType: props.Value.ContentType);
            };
        }

    }
}
