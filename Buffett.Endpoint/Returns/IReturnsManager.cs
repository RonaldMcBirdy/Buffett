using Buffett.Contracts.Models;

namespace Buffett.Endpoint.Returns
{
    public interface IReturnsManager
    {
        public Task<List<StockDailyReturn>> GetReturnsAsync(DateTime fromDate, DateTime toDate, string ticker);
    }
}
