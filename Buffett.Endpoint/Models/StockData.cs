using System.Text.Json.Serialization;

namespace Buffett.Endpoint.Models
{
    public class StockData
    {
        [JsonPropertyName("Meta Data")]
        public MetaData MetaData { get; set; }
        [JsonPropertyName("Time Series (Daily)")]
        public Dictionary<string, DailyStockInfo> TimeSeriesDaily { get; set; }
        [JsonPropertyName("Error Message")]
        public string Error { get; set; }
        public DateTime LastCacheRefresh { get; set; }

        private List<DateTime> sortedDates = new List<DateTime>();
        public List<DateTime> GetSortedDatesDescending()
        {
            if (sortedDates.Count > 0)
            {
                return sortedDates;
            }

            try
            {
                if (TimeSeriesDaily != null)
                {
                    sortedDates = TimeSeriesDaily.Keys
                        .Select(dateString => DateTime.Parse(dateString))
                        .OrderByDescending(date => date)
                        .ToList(); 
                    return sortedDates;
                }
                else
                {
                    return new List<DateTime>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing date time for ticker {MetaData.Symbol} with error: {ex.Message}");
                throw;
            }
        }
    }
}
