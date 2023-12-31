﻿using Buffett.Contracts.Query;
using Buffett.Endpoint.Settings;
using Buffett.Endpoint.Validators;
using Microsoft.Extensions.Options;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Buffett.Endpoint.Returns
{
    public class ReturnsQueryHandler : Service
    {
        private readonly IReturnsManager _returnsManager;
        public ReturnsQueryHandler(IReturnsManager returnsManager) 
        {
            _returnsManager = returnsManager;
        }
        public async Task<GetReturnsResponse> Get(GetReturnsQuery query)
        {
            if (query.FromDate == null && query.ToDate == null)
            {
                query.FromDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
                query.ToDate = DateTime.UtcNow;
            }
            else
            {
                DateTimeValidator.ValidateQuery(query.FromDate, query.ToDate);
            }

            var returnsList = await _returnsManager.GetReturnsAsync(query.FromDate.Value, query.ToDate.Value, query.Ticker);
            return new GetReturnsResponse
            {
                StockDailyReturnsList = returnsList
            };
        }
    }
}
