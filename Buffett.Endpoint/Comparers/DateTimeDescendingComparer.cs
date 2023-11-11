namespace Buffett.Endpoint.Comparers
{
    public class DateTimeDescendingComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return y.Date.CompareTo(x.Date);
        }
    }
}
