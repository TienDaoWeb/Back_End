using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TienDaoAPI.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly Cloudinary _cloudinary;
        public ImageStorageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var uploadparams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
            };

            var result = _cloudinary.Upload(uploadparams);

            if (result.Error != null)
            {
                throw new Exception($"Cloudinary error occured: {result.Error.Message}");
            }

            return result.SecureUrl.ToString();
        }
    }
}
