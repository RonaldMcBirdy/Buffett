namespace Buffett.Endpoint.Alpha
{
    public interface IAlphaManager
    {
        Task<Contracts.Models.Alpha> CalculateAlphaForTicker(DateTime fromDate, DateTime toDate, string ticker, string treasuryMaturity);
    }
}
