using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Buffett.Endpoint.Models
{
    public class DailyStockInfo
    {
        [JsonPropertyName("1. open")]
        public string OpenString { get; set; }
        [JsonPropertyName("2. high")]
        public string HighString { get; set; }
        [JsonPropertyName("3. low")]
        public string LowString { get; set; }
        [JsonPropertyName("4. close")]
        public string CloseString { get; set; }
        [JsonPropertyName("5. volume")]
        public string Volume { get; set; }

        public decimal Open
        {
            get => ParseDecimal(OpenString, "OpenString");
        }

        public decimal High
        {
            get => ParseDecimal(HighString, "HighString");
        }

        public decimal Low
        {
            get => ParseDecimal(LowString, "LowString");
        }

        public decimal Close
        {
            get => ParseDecimal(CloseString, "CloseString");
        }

        private decimal ParseDecimal(string value, string propertyName)
        {
            if (decimal.TryParse(value, out var result))
            {
                return result;
            }
            throw new InvalidOperationException($"Unable to parse '{propertyName}' property as decimal.");
        }
    }
}
