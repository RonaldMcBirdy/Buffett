using Buffett.Contracts.Models;
using ServiceStack;
using System.Collections.Generic;

namespace Buffett.Contracts.Query
{
    public class GetReturnsResponse
    {
        public List<StockDailyReturn> StockDailyReturnsList { get; set; }
    }
}
