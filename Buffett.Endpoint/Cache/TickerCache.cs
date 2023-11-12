using Buffett.Endpoint.Clients;
using Buffett.Endpoint.Constants;
using Buffett.Endpoint.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Buffett.Endpoint.Cache
{
    public class TickerCache : ITickerCache
    {
        IAlphaVantageClient _alphaVantageClient;
        IMemoryCache _memoryCache;

        public TickerCache(IAlphaVantageClient alphaVantageClient, IMemoryCache memoryCache)
        {
            _alphaVantageClient = alphaVantageClient;
            _memoryCache = memoryCache;
        }

        public async Task<StockData> GetTimeSeriesAsync(string ticker)
        {
            if (_memoryCache.TryGetValue(ticker, out StockData tickerTimeSeries))
            {
                if (CacheNeedsRefresh(tickerTimeSeries.LastCacheRefresh))
                {
                    tickerTimeSeries = await RefreshCache(ticker);
                }

                return tickerTimeSeries;
            }
            else
            {
                var stockData = await RefreshCache(ticker);
                return stockData;
            }
        }

        public async Task PreloadCacheAsync()
        {
            await RefreshCache(StockConstants.Spy);
        }

        private async Task<StockData> RefreshCache(string ticker)
        {
            var stockData = await _alphaVantageClient.GetReturnsAsync(ticker);
            stockData.LastCacheRefresh = DateTime.UtcNow;
            _memoryCache.Set(ticker.ToLower(), stockData);
            return stockData;
        }

        private bool CacheNeedsRefresh(DateTime lastRefreshTime)
        {
            return lastRefreshTime.Date < DateTime.UtcNow.Date;
        }
    }
}
