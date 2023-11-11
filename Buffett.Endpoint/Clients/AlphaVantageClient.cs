using Buffett.Endpoint.Models;
using Buffett.Endpoint.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Buffett.Endpoint.Clients
{
    public class AlphaVantageClient : IAlphaVantageClient
    {
        private readonly IOptionsMonitor<BuffettSettings> _options;
        private readonly HttpClient _httpClient;
        private readonly string AlphaVantageGetTickerBaseQuery = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&outputsize=full";
        public AlphaVantageClient(IOptionsMonitor<BuffettSettings> options) 
        {
            _options = options;
            _httpClient = new HttpClient();
        }

        public async Task<StockData> GetReturnsAsync(string ticker)
        {
            try
            {
                var response = await _httpClient.GetAsync(QueryBuilder(ticker));
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();

                // string jsonString = await File.ReadAllTextAsync("C:\\project\\Buffett\\ExampleTickerresponse.json");
                var stockData = JsonSerializer.Deserialize<StockData>(jsonString);

                if (stockData.Error != null)
                {
                    throw new Exception($"Ticker : {ticker}, {stockData.Error}");
                }
                return stockData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception when fetching stock data for ticker {ticker}: {ex.Message}");
                throw;
            }
        }

        private string QueryBuilder(string ticker)
        {
            var x = AlphaVantageGetTickerBaseQuery +
                "&symbol=" + ticker + "&apikey=" + _options.CurrentValue.ApiKey;
            return x;
        }
    }
}
