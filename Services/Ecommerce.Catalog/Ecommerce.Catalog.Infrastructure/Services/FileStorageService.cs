using Azure.Storage.Blobs;
using Ecommerce.Catalog.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Catalog.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public FileStorageService(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("AzureStorage:ConnectionString").Value;
        var containerName = configuration.GetSection("AzureStorage:ShareName").Value;
        _blobContainerClient = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string?> UploadFileAsync(string fileName, Stream fileContent)
    {
        string url = null;
        try
        {
            var guidName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            var blobClient = _blobContainerClient.GetBlobClient(guidName);

            await blobClient.UploadAsync(fileContent, true);

            //_logger.LogInformation("File {FileName} uploaded to {BlobUri}", fileName, url);

            url = blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            //return ex.InnerException?.Message ?? ex.Message;
        }
        return url;
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        try
        {
            Uri uri = new(fileUrl);
            var fileName = System.IO.Path.GetFileName(uri.LocalPath);
            var response = await _blobContainerClient.DeleteBlobAsync(fileName, Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            //_logger.LogInformation($"File {request.FileName} deleted: {!response.IsError} response: {response.ReasonPhrase}");

            return !response.IsError;
        }
        catch (Exception ex)
        {
            //_logger.LogError($"DeleteFile name:{request.FileName} error:{ex.Message}");
            return false;
        }
    }
}