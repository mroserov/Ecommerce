namespace Ecommerce.Catalog.Domain.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string? fileName, Stream fileContent);
    Task<bool> DeleteFileAsync(string fileUrl);
}

