using ServiceStack;
using System;

namespace Buffett.Contracts.Queries
{
    [Route("/alpha/{Ticker}", "GET")]
    public class GetAlphaQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Ticker { get; set; }
        public string TreasuryMaturity { get; set; }
    }
}
