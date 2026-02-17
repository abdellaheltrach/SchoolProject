using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using School.Service.Services;
using System.Text;

namespace School.Tests.Services
{
    public class FileServiceIntegrationTests : IDisposable
    {
        private readonly string _webRootPath;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly FileService _fileService;

        public FileServiceIntegrationTests()
        {
            _webRootPath = Path.Combine(Path.GetTempPath(), "SchoolProjectTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_webRootPath);

            _envMock = new Mock<IWebHostEnvironment>();
            _envMock.Setup(e => e.WebRootPath).Returns(_webRootPath);

            _fileService = new FileService(_envMock.Object);
        }

        #region UploadImage Tests
        [Fact]
        public async Task UploadImage_ShouldReturnPath_WhenFileIsValid()
        {
            // Arrange
            var content = "fake image content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.jpg");
            fileMock.Setup(f => f.Length).Returns(bytes.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns<Stream, CancellationToken>((s, _) => stream.CopyToAsync(s));

            // Act
            var result = await _fileService.UploadImage("Instructors", fileMock.Object);

            // Assert
            result.Should().StartWith("/Instructors/");
            var physicalPath = Path.Combine(_webRootPath, result.TrimStart('/'));
            File.Exists(physicalPath).Should().BeTrue();
        }

        [Fact]
        public async Task UploadImage_ShouldReturnNoImage_WhenFileIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _fileService.UploadImage("Instructors", fileMock.Object);

            // Assert
            result.Should().Be("NoImage");
        }

        [Fact]
        public async Task UploadImage_ShouldReturnFailedToUploadImage_OnException()
        {
            // Arrange
            // Force an invalid path to trigger an exception during directory creation or file stream opening
            _envMock.Setup(e => e.WebRootPath).Returns("Z:\\invalid_path");

            var content = "data";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.jpg");
            fileMock.Setup(f => f.Length).Returns(bytes.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Returns<Stream, CancellationToken>((s, _) => stream.CopyToAsync(s));

            // Act
            var result = await _fileService.UploadImage("Instructors", fileMock.Object);

            // Assert
            result.Should().Be("FailedToUploadImage");
        }
        #endregion

        public void Dispose()
        {
            if (Directory.Exists(_webRootPath))
            {
                try
                {
                    Directory.Delete(_webRootPath, true);
                }
                catch
                {
                    // Ignore cleanup errors in tests
                }
            }
        }
    }
}
