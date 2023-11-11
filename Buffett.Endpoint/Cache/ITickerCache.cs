using Buffett.Endpoint.Models;

namespace Buffett.Endpoint.Cache
{
    public interface ITickerCache
    {
        public Task<StockData> GetTimeSeriesAsync(string ticker);
    }
}
