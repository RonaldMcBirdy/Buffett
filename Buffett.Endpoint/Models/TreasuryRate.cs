using System.Text.Json.Serialization;

namespace Buffett.Endpoint.Models
{
    public class TreasuryRate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("interval")]
        public string Interval { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("data")]
        public List<TreasuryRateData> Data { get; set; }

        [JsonPropertyName("Error Message")]
        public string Error { get; set; }

        public DateTime LastCacheRefresh { get; set; }

    }
}
