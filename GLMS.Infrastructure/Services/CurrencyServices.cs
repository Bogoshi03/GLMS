using GLMS.Core.Interfaces;
using Newtonsoft.Json;
using System.Text.Json;

namespace GLMS.Core.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertUsdToZar(decimal usdAmount)
        {
            try
            {
                string url =
                    "https://open.er-api.com/v6/latest/USD";

                var response =
                    await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json =
                    await response.Content.ReadAsStringAsync();

                using JsonDocument document =
                    JsonDocument.Parse(json);

                decimal zarRate =
                    document.RootElement
                        .GetProperty("rates")
                        .GetProperty("ZAR")
                        .GetDecimal();

                return usdAmount * zarRate;
            }
            catch
            {
                // fallback if API fails
                return usdAmount * 18.5m;
            }
        }
    }
}