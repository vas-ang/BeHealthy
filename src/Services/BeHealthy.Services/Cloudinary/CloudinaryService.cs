namespace BeHealthy.Services.Cloudinary
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

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

            ImageUploadParams uploadParams = new ImageUploadParams
            {
                Folder = folder,
                Transformation = new Transformation().Crop("limit").Width(800).Height(600),
                File = new FileDescription(name, stream),
            };

            UploadResult uploadResult = await this.cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUri.AbsoluteUri;
        }
    }
}
