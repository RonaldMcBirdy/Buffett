using Buffett.Endpoint.Models;

namespace Buffett.Endpoint.Cache
{
    public interface ITreasuryCache
    {
        Task<TreasuryRate> GetTreasuryAsync(string treasuryMaturity);
        Task PreloadCacheAsync();
    }
}
