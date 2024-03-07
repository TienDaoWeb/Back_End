using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IFirebaseStorageService
{
    Task<Uri> UploadFile(string name, IFormFile file);
}