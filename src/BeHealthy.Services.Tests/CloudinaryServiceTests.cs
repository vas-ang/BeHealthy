namespace BeHealthy.Services.Tests
{
    using BeHealthy.Services.Cloudinary;
    using Moq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Xunit;

    public class CloudinaryServiceTests
    {
        [Fact]
        public async Task UploadImageReturnsUri()
        {
            var cloudinary = new Mock<Cloudinary>(MockBehavior.Loose);



            var cloudinaryService = new CloudinaryService(cloudinary.Object);
        }
    }
}
