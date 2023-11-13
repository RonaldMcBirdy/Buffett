using Buffett.Contracts.Models;
using Buffett.Endpoint.Cache;
using Buffett.Endpoint.Comparers;
using Buffett.Endpoint.Helpers;
using Buffett.Endpoint.Models;

namespace Buffett.Endpoint.Returns
{
    public class ReturnsManager : IReturnsManager
    {
        ITickerCache _tickerCache;

        public ReturnsManager(ITickerCache tickerCache)
        { 
            _tickerCache = tickerCache;
        }

        public async Task<List<StockDailyReturn>> GetReturnsAsync(DateTime fromDate, DateTime toDate, string ticker)
        {
            var stockSeries = await _tickerCache.GetTimeSeriesAsync(ticker);
            var sortedDates = stockSeries.GetSortedDatesDescending();
            var index = SortedDateListHelper.FindIndex(sortedDates, toDate, true);

            List<StockDailyReturn> stockDailyReturns = new List<StockDailyReturn>();
            var currentDate = sortedDates[index];
            while (currentDate.Date >= fromDate.Date)
            {
                stockDailyReturns.Add(new StockDailyReturn
                {
                    Day = currentDate.Date.ToString("yyyy-MM-dd"),
                    DailyReturn = GetReturnForDay(stockSeries, currentDate)
                });

                index++;
                if (index == sortedDates.Count)
                {
                    break;
                }
                currentDate = sortedDates[index];

            }

            return stockDailyReturns;
        }

        private decimal GetReturnForDay(StockData stockData, DateTime dateToRetrieve)
        {
            if (stockData.TimeSeriesDaily.TryGetValue(dateToRetrieve.ToString("yyyy-MM-dd"),
                out var dailyStockInfo))
            {
                return dailyStockInfo.Close - dailyStockInfo.Open;
            }
            else
            {
                throw new Exception("Unexpected error finding date in TimeSeries, please reach out to team for resolution");
            }
        }
    }
}
