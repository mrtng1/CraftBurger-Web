using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace service.Services.Blob;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
    }

    public async Task<string> UploadImageAsync(string containerName, IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobName = Guid.NewGuid().ToString(); // Use a unique name for each image

        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.ToString();
    }
}