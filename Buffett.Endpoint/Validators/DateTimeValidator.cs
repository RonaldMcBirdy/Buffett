using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffett.Endpoint.Validators
{
    public static class DateTimeValidator
    {
        public static void ValidateQuery(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null)
            {
                throw new ArgumentException("FromDate is invalid or missing");
            }
            if (toDate == null)
            {
                throw new ArgumentException("ToDate is invalid or missing");
            }
            if (!IsTodayOrBefore(toDate))
            {
                throw new ArgumentException("ToDate cannot be in the future");
            }
            if (fromDate > toDate)
            {
                throw new ArgumentException("FromDate cannot be larger than ToDate");
            }
        }

        private static bool IsTodayOrBefore(DateTime? toDate)
        {
            return toDate?.Date <= DateTime.Now.Date;
        }
    }
}
