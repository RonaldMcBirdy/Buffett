using Buffett.Contracts.Query;
using Buffett.Endpoint.Models;

namespace Buffett.Endpoint.Clients
{
    public interface IAlphaVantageClient
    {
        public Task<StockData> GetReturnsAsync(string ticker);
        public Task<TreasuryRate> GetTreasuryAsync(string treasuryMaturity);
    }
}
