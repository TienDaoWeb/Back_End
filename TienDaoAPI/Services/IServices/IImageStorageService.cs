public interface IImageStorageService
{
    Task<string> UploadImageAsync(IFormFile file);
}