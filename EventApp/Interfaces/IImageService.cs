namespace EventApp.Services.Media;

public interface IImageService
{
    Task<(string FileName, string Url)?> UploadImageAsync(IFormFile file);
    Task<bool> DeleteImageAsync(string publicId);
}
