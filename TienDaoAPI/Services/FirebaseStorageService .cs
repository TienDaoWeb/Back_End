using Google.Cloud.Storage.V1;

namespace TienDaoAPI.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private const string BucketName = "tiendaoapi.appspot.com";
        public FirebaseStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }
        public async Task<Uri> UploadFile(string name, IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var blob = await _storageClient.UploadObjectAsync(BucketName,
                $"images/{name}", file.ContentType, stream);
            var photoUri = new Uri(blob.MediaLink);
            return photoUri;
        }
    }
}
