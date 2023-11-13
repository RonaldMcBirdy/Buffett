using Buffett.Endpoint.Models;
using Buffett.Endpoint.Settings;
using Microsoft.Extensions.Options;
using ServiceStack.Auth;
using System.Text.Json;

namespace Buffett.Endpoint.Clients
{
    public class AlphaVantageClient : IAlphaVantageClient
    {
        private readonly IOptionsMonitor<BuffettSettings> _options;
        private readonly HttpClient _httpClient;
        private readonly string AlphaVantageGetTickerBaseQuery = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&outputsize=full";
        private readonly string AlphaVantageGetTreasuryBaseQuery = "https://www.alphavantage.co/query?function=TREASURY_YIELD&interval=monthly";

        public AlphaVantageClient(IOptionsMonitor<BuffettSettings> options) 
        {
            _options = options;
            _httpClient = new HttpClient();
        }

        public async Task<StockData> GetReturnsAsync(string ticker)
        {
            try
            {
                var response = await _httpClient.GetAsync(ReturnsQueryBuilder(ticker));
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();

                var stockData = JsonSerializer.Deserialize<StockData>(jsonString);

                if (stockData.Error != null)
                {
                    throw new Exception($"Ticker : {ticker} -> {stockData.Error}");
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

        public async Task<TreasuryRate> GetTreasuryAsync(string treasuryMaturity)
        {
            try
            {
                var response = await _httpClient.GetAsync(TreasuryQueryBuilder(treasuryMaturity));
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                var treasuryData = JsonSerializer.Deserialize<TreasuryRate>(jsonString);

                if (treasuryData.Error != null)
                {
                    throw new Exception($"Treasury Maturity : {treasuryMaturity} -> {treasuryData.Error}");
                }
                return treasuryData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when fetching treasury data for treasuryMaturity {treasuryMaturity}: {ex.Message}");
                throw;
            }
        }

        private string ReturnsQueryBuilder(string ticker)
        {
            return AlphaVantageGetTickerBaseQuery +
                "&symbol=" + ticker + "&apikey=" + _options.CurrentValue.ApiKey;
        }

        private string TreasuryQueryBuilder(string treasuryMaturity)
        {
            var x = AlphaVantageGetTreasuryBaseQuery +
                "&maturity=" + treasuryMaturity + "&apikey=" + _options.CurrentValue.ApiKey;
            return x;
        }
    }
}
