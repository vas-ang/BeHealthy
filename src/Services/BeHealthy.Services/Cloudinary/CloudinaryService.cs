namespace BeHealthy.Services.Cloudinary
{
    using System.IO;
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
            using MemoryStream memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            ImageUploadParams imageParameters = new ImageUploadParams { Folder = folder, File = new FileDescription(name, memoryStream) };
            var imageUploadedResult = await this.cloudinary.UploadAsync(imageParameters);

            return imageUploadedResult.SecureUri.AbsoluteUri;
        }
    }
}
