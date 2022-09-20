using Azure.Storage.Blobs;

namespace Birdy.API.Services.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<Uri> Upload(string storageFileName, string fileType, byte[] fileStream);
        Task<(byte[] fileData, string mimeType)> Retrieve(string storageFileName);
    }
}
