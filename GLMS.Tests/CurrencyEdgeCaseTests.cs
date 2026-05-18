using System.Net.Http;
using System.Threading.Tasks;
using GLMS.Core.Services;
using Xunit;

namespace GLMS.Tests
{
    public class CurrencyEdgeCaseTests
    {
        [Fact]
        public async Task ConvertUsdToZar_ZeroUsd_ShouldReturnZero()
        {
            // Arrange
            var httpClient = new HttpClient();

            var service = new CurrencyService(httpClient);

            decimal usd = 0m;

            // Act
            decimal result = await service.ConvertUsdToZar(usd);

            // Assert
            Assert.Equal(0m, result);
        }
    }
}
