using Buffett.Endpoint.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffett.Endpoint.Helpers
{
    public static class SortedDateListHelper
    {
        public static int FindIndex(List<DateTime> sortedList, DateTime target)
        {
            int index = sortedList.BinarySearch(target, new DateTimeDescendingComparer());
            var foundIndex = index < 0 ? ~index : index;

            if (foundIndex >= sortedList.Count)
            {
                throw new ArgumentException("There is no data between these dates");
            }
            return foundIndex;
        }
    }
}
