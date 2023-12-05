namespace service.Interfaces.Blob;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, Stream fileStream, string fileName);
    Task<bool> DeleteFileAsync(string containerName, string fileName);
}