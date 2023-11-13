using Buffett.Endpoint.Comparers;
using System;
namespace Buffett.Endpoint.Helpers
{
    public static class SortedDateListHelper
    {
        public static int FindIndex(List<DateTime> sortedList, DateTime target, bool isToDate)
        {
            int index = sortedList.BinarySearch(target, new DateTimeDescendingComparer());
            var foundIndex = index < 0 ? ~index : index;

            if (foundIndex >= sortedList.Count)
            {
                // if isToDate and ob throw exception, if from date return last index in list
                if (isToDate)
                {
                    throw new Exception("There is no data between these dates");
                }
                else
                {
                    foundIndex = sortedList.Count - 1;
                }
            }
            return foundIndex;
        }
    }
}
