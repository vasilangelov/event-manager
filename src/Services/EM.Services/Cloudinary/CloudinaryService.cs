namespace EM.Services.Cloudinary
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    [TransientService]
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(string name, string folder, IFormFile image)
        {
            using var stream = image.OpenReadStream();

            ImageUploadParams uploadParams = new()
            {
                Folder = folder,
                Transformation = new Transformation().AspectRatio(16, 9).Crop("crop"),
                File = new FileDescription(name, stream),
            };

            ImageUploadResult uploadResult = await this.cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
