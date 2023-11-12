using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Buffett.Endpoint.Models
{
    public class TreasuryRateData
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("value")]
        public string ValueString { get; set; }

        public decimal Value
        {
            get => ParseDecimal(ValueString, "ValueString");
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
