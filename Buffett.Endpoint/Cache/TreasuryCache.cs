using Buffett.Endpoint.Clients;
using Buffett.Endpoint.Constants;
using Buffett.Endpoint.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Buffett.Endpoint.Cache
{
    public class TreasuryCache : ITreasuryCache
    {
        IAlphaVantageClient _alphaVantageClient;
        IMemoryCache _memoryCache;

        public TreasuryCache(IAlphaVantageClient alphaVantageClient, IMemoryCache memoryCache)
        {
            _alphaVantageClient = alphaVantageClient;
            _memoryCache = memoryCache;
        }

        public async Task<TreasuryRate> GetTreasuryAsync(string treasuryMaturity)
        {
            if (_memoryCache.TryGetValue(treasuryMaturity, out TreasuryRate treasuryRate))
            {
                if (CacheNeedsRefresh(treasuryRate.LastCacheRefresh))
                {
                    treasuryRate = await RefreshCache(treasuryMaturity);
                }

                return treasuryRate;
            }
            else
            {
                var stockData = await RefreshCache(treasuryMaturity);
                return stockData;
            }
        }

        public async Task PreloadCacheAsync()
        {
            foreach (var maturity in TreasuryMaturityConstants.AllTreasuryYields)
            {
                await RefreshCache(maturity);
            }
        }

        private async Task<TreasuryRate> RefreshCache(string treasuryMaturity)
        {
            var treasuryRate = await _alphaVantageClient.GetTreasuryAsync(treasuryMaturity);
            treasuryRate.LastCacheRefresh = DateTime.UtcNow;
            _memoryCache.Set(treasuryMaturity.ToLower(), treasuryRate);
            return treasuryRate;
        }

        private bool CacheNeedsRefresh(DateTime lastRefreshTime)
        {
            return lastRefreshTime.Date < DateTime.UtcNow.Date;
        }
    }
}
