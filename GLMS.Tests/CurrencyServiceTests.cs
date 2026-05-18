using System.Net.Http;
using System.Threading.Tasks;
using GLMS.Core.Services;
using Xunit;

namespace GLMS.Tests
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task ConvertUsdToZar_ShouldReturnCorrectValue()
        {
            // Arrange
            var httpClient = new HttpClient();

            var service = new CurrencyService(httpClient);

            decimal usd = 10m;

            // Act
            decimal result = await service.ConvertUsdToZar(usd);

            // Assert
            Assert.True(result > 0);
        }
    }
}