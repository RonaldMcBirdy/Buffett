using Buffett.Endpoint.Alpha;
using Buffett.Endpoint.Cache;
using Buffett.Endpoint.Constants;
using Buffett.Endpoint.Models;
using Moq;
using NUnit.Framework;

namespace Buffett.UnitTests.Managers
{
    public class AlphaManagerTests
    {
        private AlphaManager _alphaManager;
        private Mock<ITickerCache> _mockTickerCache;
        private Mock<ITreasuryCache> _mockTreasuryCache;

        private string day1 = new DateTime(2023, 11, 9).ToString("yyyy-MM-dd");
        private string day2 = new DateTime(2023, 11, 6).ToString("yyyy-MM-dd");
        private string day3 = new DateTime(2023, 11, 5).ToString("yyyy-MM-dd");
        private string exampleTicker = "EXPL";

        private DateTime tDate1 = new DateTime(2023, 11, 1);
        private DateTime tDate2 = new DateTime(2023, 10, 1);
        private DateTime tDate3 = new DateTime(2023, 9, 1);

        private StockData spyStockData;
        private StockData targetCompanyStockData;
        private TreasuryRate tRate;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockTickerCache = new Mock<ITickerCache>();
            _mockTreasuryCache = new Mock<ITreasuryCache>();

            spyStockData = new StockData
            {
                TimeSeriesDaily = new Dictionary<string, DailyStockInfo>
                {
                    {
                        day1, new DailyStockInfo
                        {
                            CloseString = "410.12",
                            OpenString = "410.10"
                        }
                    },
                    {
                        day2, new DailyStockInfo
                        {
                            CloseString = "410.10",
                            OpenString = "402.12"
                        }
                    },
                    {
                        day3, new DailyStockInfo
                        {
                            CloseString = "402.12",
                            OpenString = "403.50"
                        }
                    }
                }
            };

            targetCompanyStockData = new StockData
            {
                TimeSeriesDaily = new Dictionary<string, DailyStockInfo>
                {
                    {
                        day1, new DailyStockInfo
                        {
                            CloseString = "22.12",
                            OpenString = "29.10"
                        }
                    },
                    {
                        day2, new DailyStockInfo
                        {
                            CloseString = "29.10",
                            OpenString = "18.12"
                        }
                    },
                    {
                        day3, new DailyStockInfo
                        {
                            CloseString = "18.12",
                            OpenString = "17.50"
                        }
                    }
                }
            };

            tRate = new TreasuryRate
            {
                Data = new List<TreasuryRateData>
                {
                    new TreasuryRateData
                    {
                        Date = tDate1,
                        ValueString = "4.50"
                    },
                    new TreasuryRateData
                    {
                        Date = tDate2,
                        ValueString = "3.50"
                    },
                    new TreasuryRateData
                    {
                        Date = tDate3,
                        ValueString = "2.50"
                    }
                }
            };

            _mockTickerCache.Setup(x => x.GetTimeSeriesAsync(StockConstants.Spy)).ReturnsAsync(spyStockData);
            _mockTickerCache.Setup(x => x.GetTimeSeriesAsync(exampleTicker)).ReturnsAsync(targetCompanyStockData);
            _mockTreasuryCache.Setup(x => x.GetTreasuryAsync(It.IsAny<string>())).ReturnsAsync(tRate);
            _alphaManager = new AlphaManager(_mockTickerCache.Object, _mockTreasuryCache.Object);
        }

        [Test]
        public async Task AlphaCalculateHappyPath()
        {
            DateTime fromDate = new DateTime(2023, 11, 1);
            DateTime toDate = new DateTime(2023, 12, 20);

            var alpha = await _alphaManager.CalculateAlphaForTicker(fromDate, toDate, exampleTicker, "2Year");

            var marketReturn = Math.Round((spyStockData.TimeSeriesDaily[day1].Close / spyStockData.TimeSeriesDaily[day3].Close) - 1, 4);
            var targetReturn = Math.Round((targetCompanyStockData.TimeSeriesDaily[day1].Close / targetCompanyStockData.TimeSeriesDaily[day3].Close) - 1, 4);
            var riskFreeReturn = Math.Round(tRate.Data[0].Value / 100, 4);
            Assert.AreEqual(marketReturn, alpha.MarketReturn);
            Assert.AreEqual(targetReturn, alpha.ActualReturn);
            Assert.AreEqual(riskFreeReturn, alpha.RiskFreeReturn);
            _mockTickerCache.Verify(x => x.GetTimeSeriesAsync(StockConstants.Spy), Times.Once);
            _mockTickerCache.Verify(x => x.GetTimeSeriesAsync(exampleTicker), Times.Once);
            _mockTreasuryCache.Verify(x => x.GetTreasuryAsync(It.IsAny<string>()), Times.Once);
        }


        [Test]
        public async Task AlphaCalculateNoTreasuryFound()
        {
            _mockTreasuryCache.Setup(x => x.GetTreasuryAsync(It.IsAny<string>())).ReturnsAsync(new TreasuryRate
            {
                Data = new List<TreasuryRateData>
                {
                    new TreasuryRateData()
                    {
                        Date = new DateTime(2022, 1, 1)
                    }
                }
            });

            DateTime fromDate = new DateTime(2023, 11, 1);
            DateTime toDate = new DateTime(2023, 12, 20);

            Assert.ThrowsAsync<Exception>(() => _alphaManager.CalculateAlphaForTicker(fromDate, toDate, exampleTicker, "2Year"));
        }
    }
}
