ASSUMPTIONS:
alphavantage is source of truth on dates, we are not cross checking market days

Considerations/Improvements:
 - Fetching all from alphavantage (20+ years) currently but we could use the limited function to limit memory in cache.
 - Query building could be cleaner or more configurable if we expect changes in the future to the api
 - I used an in memory cache -> monitoring or evection based on available memory could be added but overall concern is fairly low (depending on traffic)
 - Secure api secret in system variable
 - Auto calculation of treasury maturity based on given interval
 - using spy as bench mark ok?
 - Validation is a little choppy
 - Looks like alphavantage doesnt take into account stock splits so something like apple's alpha values are not correct