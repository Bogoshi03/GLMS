using GLMS.Core.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GLMS.Tests
{
    public class FileValidationServiceTests
    {
        [Fact]
        public void IsValidFile_ShouldReturnFalse_ForExeFile()
        {
            // Arrange
            var service = new FileValidationService();

            var file = new FormFile(
                baseStream: Stream.Null,
                baseStreamOffset: 0,
                length: 100,
                name: "Data",
                fileName: "virus.exe"
            );

            // Act
            bool result = service.IsValidFile(file);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidFile_ShouldReturnTrue_ForPdfFile()
        {
            // Arrange
            var service = new FileValidationService();

            var file = new FormFile(
                baseStream: Stream.Null,
                baseStreamOffset: 0,
                length: 100,
                name: "Data",
                fileName: "contract.pdf"
            );

            // Act
            bool result = service.IsValidFile(file);

            // Assert
            Assert.True(result);
        }
    }
}