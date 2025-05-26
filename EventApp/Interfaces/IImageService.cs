using Microsoft.AspNetCore.Http;

namespace EventApp.Services.Media;

public interface IImageService
{
    Task<(string FileName, string Url)> UploadImageAsync(IFormFile file);
}
