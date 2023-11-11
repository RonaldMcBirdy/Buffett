using ServiceStack;
using System;

namespace Buffett.Contracts.Query
{
    [Route("/returns/{Ticker}", "GET")]
    public class GetReturnsQuery : IReturn<GetReturnsResponse>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Ticker { get; set; }
    }
}
