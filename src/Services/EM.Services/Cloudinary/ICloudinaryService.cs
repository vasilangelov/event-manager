namespace EM.Services.Cloudinary
{
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(string name, string folder, IFormFile image);
    }
}
