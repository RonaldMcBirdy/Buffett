using Buffett.Contracts.Queries;
using Buffett.Contracts.Query;
using Buffett.Endpoint.Constants;
using Buffett.Endpoint.Validators;
using ServiceStack;

namespace Buffett.Endpoint.Alpha
{
    public class GetAlphaQueryHandler : Service
    {
        private readonly IAlphaManager _alphaManager;
        public GetAlphaQueryHandler(IAlphaManager alphaManager)
        {
            _alphaManager = alphaManager;
        }
        public async Task<GetAlphaResponse> Get(GetAlphaQuery query)
        {
            if (query.FromDate == null && query.ToDate == null)
            {
                query.FromDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
                query.ToDate = DateTime.UtcNow;
            }
            else
            {
                DateTimeValidator.ValidateQuery(query.FromDate, query.ToDate);
                ValidateTreasuryMaturity(query.TreasuryMaturity);
            }

            var alpha = await _alphaManager.CalculateAlphaForTicker(query.FromDate.Value, query.ToDate.Value, query.Ticker, query.TreasuryMaturity);
            return new GetAlphaResponse
            {
                Alpha = alpha
            };
        }

        private void ValidateTreasuryMaturity(string treasuryMaturity)
        {
            if (!TreasuryMaturityConstants.AllTreasuryYields.Contains(treasuryMaturity))
            {
                throw new ArgumentException("TreasuryMaturity must be one of 3month, 2year, 5year, 7year, 10year, or 30year");
            }
        }
    }
}
