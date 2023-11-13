Service:
Buffett (after Warren) is a service that can both return a stocks daily returns as well as calculate alpha of a stock. Buffett has an in memory caching system
partially for speed but in this case to save api calls since this data will change at most once a day we have a strong use case for caching. The caching
auto refreshes if its been a day since the cache last refreshed its values. Additionally, the cache is prefilled on startup for everything we will need for alpha
calculations (spy and treasury time series).

ASSUMPTIONS:
Alphavantage is source of truth on dates and values, we are not cross checking market days or values for accuracy

Considerations/Improvements:
 - I used an in memory cache -> monitoring or evection based on available memory could be added but overall concern is fairly low (depending on traffic)
 - commiting appsecret in code is not good practice, this would be safely secured
 - using spy as bench mark ok? Could fairly easily add in optional bench mark data ticker
 - During testing it looks like alphavantage doesnt take into account stock splits so something like amazons alpha values are not correct
 - more thorough unit testing -> validations, alpha calculations especially
 - Could add a lot more info around returns as well depending on business need