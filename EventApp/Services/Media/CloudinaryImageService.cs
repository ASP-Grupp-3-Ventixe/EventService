using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventApp.Services.Media;
using EventApp.Settings;
using Microsoft.Extensions.Options;

namespace EventApp.Services;

public class CloudinaryImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageService(IOptions<CloudinarySettings> settings)
    {
        var acc = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
            settings.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<(string FileName, string Url)> UploadImageAsync(IFormFile file)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = "event-images"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Cloudinary upload failed");

        return (uploadResult.PublicId, uploadResult.SecureUrl.AbsoluteUri);
    }
}
