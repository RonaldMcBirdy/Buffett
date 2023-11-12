using Buffett.Endpoint.Cache;
using Buffett.Endpoint.Constants;
using Buffett.Endpoint.Helpers;
using Buffett.Endpoint.Models;

namespace Buffett.Endpoint.Alpha
{
    public class AlphaManager : IAlphaManager
    {
        private readonly ITickerCache _tickerCache;
        private readonly ITreasuryCache _treasuryCache;

        public AlphaManager(ITickerCache tickerCache, ITreasuryCache treasuryCache)
        {
            _tickerCache = tickerCache;
            _treasuryCache = treasuryCache;
        }

        public async Task<Contracts.Models.Alpha> CalculateAlphaForTicker(DateTime fromDate, DateTime toDate, string ticker, string treasuryMaturity)
        {
            var inputStock = await _tickerCache.GetTimeSeriesAsync(ticker);
            var marketStock = await _tickerCache.GetTimeSeriesAsync(StockConstants.Spy);
            var treasuryRate = await _treasuryCache.GetTreasuryAsync(treasuryMaturity);

            var inputStockActualReturn = CalculateReturn(inputStock, fromDate, toDate);
            var benchmarkReturn = CalculateReturn(marketStock, fromDate, toDate);
            var riskFreeRate = GetTreasuryYield(treasuryRate, fromDate);
            var beta = CalculateBeta(inputStock, marketStock, fromDate, toDate);
            var alpha = CalculateAlpha(inputStockActualReturn, benchmarkReturn, riskFreeRate, beta);

            return new Contracts.Models.Alpha
            {
                ActualReturn = Math.Round(inputStockActualReturn, 4),
                MarketReturn = Math.Round(benchmarkReturn, 4),
                RiskFreeReturn = Math.Round(riskFreeRate, 4),
                Beta = Math.Round(beta, 4),
                AlphaValue = Math.Round(alpha, 4)
            };
        }

        private decimal CalculateAlpha(decimal actualReturn, decimal marketReturn, decimal riskFreeRate, decimal beta)
        {
            return actualReturn - (riskFreeRate + (beta * (marketReturn - riskFreeRate)));

        }

        // get treasury rate from beginning of inputed to show oppurtunity cost
        private decimal GetTreasuryYield(TreasuryRate treasuryRate, DateTime fromDate)
        {
            var treasuryData = treasuryRate.Data.FirstOrDefault(x => x.Date.Year == fromDate.Year
            && x.Date.Month == fromDate.Month);

            if (treasuryData == null)
            {
                throw new Exception("Could not find a corresponding treasury rate in the specified date");
            }

            // alpha vantage returns as a percent so convert to decimal
            return treasuryData.Value / 100;
        }

        private decimal CalculateReturn(StockData stock, DateTime fromDate, DateTime toDate)
        {
            var sortedDates = stock.GetSortedDatesDescending();

            var firstDateIndex = SortedDateListHelper.FindIndex(sortedDates, toDate);
            var lastDateIndex = SortedDateListHelper.FindIndex(sortedDates, fromDate);

            if (stock.TimeSeriesDaily.TryGetValue(sortedDates[firstDateIndex].ToString("yyyy-MM-dd"), out var toStockInfo) &&
                stock.TimeSeriesDaily.TryGetValue(sortedDates[lastDateIndex].ToString("yyyy-MM-dd"), out var fromStockInfo))
            {
                return (toStockInfo.Close / fromStockInfo.Close) - 1;
            }
            else
            {
                throw new Exception("Unexpected error finding date in TimeSeries, please reach out to team for resolution");
            }
        }

        private decimal CalculateBeta(StockData inputStock, StockData marketStock, DateTime fromDate, DateTime toDate)
        {
            // filter before sorting here
            var marketDaysInPeriod = inputStock.GetSortedDatesDescending()
                .Where(x => x.Date <= toDate.Date && x.Date >= fromDate.Date)
                .OrderByDescending(date => date)
                .ToList();

            var inputStockData = new List<DailyStockInfo>();
            marketDaysInPeriod.ForEach(x => inputStockData.Add(inputStock.TimeSeriesDaily[x.Date.ToString("yyyy-MM-dd")]));
            var inputStockPercentReturnsPerDay = inputStockData.Select(x => x.Close / x.Open * 100).ToList();

            var marketStockData = new List<DailyStockInfo>();
            marketDaysInPeriod.ForEach(x => marketStockData.Add(marketStock.TimeSeriesDaily[x.Date.ToString("yyyy-MM-dd")]));
            var marketStockPercentReturnsPerDay = marketStockData.Select(x => x.Close / x.Open * 100).ToList();

            var covariance = CalculateCovariance(inputStockPercentReturnsPerDay, marketStockPercentReturnsPerDay);
            var variance = CalculateVariance(marketStockPercentReturnsPerDay);
            return covariance / variance;
        }

        // https://www.investopedia.com/articles/financial-theory/11/calculating-covariance.asp
        private decimal CalculateCovariance(List<decimal> returns1, List<decimal> returns2)
        {
            // Check if the input arrays have the same length
            if (returns1.Count != returns2.Count)
            {
                throw new ArgumentException("Market data or selected stock ticker do not have the same number of days");
            }

            // Calculate means of returns
            decimal meanReturns1 = returns1.Average();
            decimal meanReturns2 = returns2.Average();

            // Calculate the sum of the product of the differences
            decimal sum = 0;
            for (int i = 0; i < returns1.Count; i++)
            {
                decimal diffReturns1 = returns1[i] - meanReturns1;
                decimal diffReturns2 = returns2[i] - meanReturns2;
                sum += diffReturns1 * diffReturns2;
            }

            // Divide by (n-1) to get covariance
            decimal covariance = sum / (returns1.Count - 1);

            return covariance;
        }

        // https://www.investopedia.com/terms/v/variance.asp
        private decimal CalculateVariance(List<decimal> returns)
        {
            // Calculate the mean of returns
            decimal meanReturns = returns.Average();

            // Calculate the sum of squared differences
            decimal sum = 0;
            for (int i = 0; i < returns.Count; i++)
            {
                decimal diff = returns[i] - meanReturns;
                sum += diff * diff;
            }

            // Divide by (n-1) to get variance
            decimal variance = sum / (returns.Count - 1);

            return variance;
        }
    }
}
