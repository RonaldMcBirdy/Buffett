using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffett.Endpoint.Constants
{
    public static class TreasuryMaturityConstants
    {
        public static string Treasury3Month { get; } = "3month";
        public static string Treasury2Year { get; } = "2year";
        public static string Treasury5Year { get; } = "5year";
        public static string Treasury7Year { get; } = "7year";
        public static string Treasury10Year { get; } = "10year";
        public static string Treasury30Year { get; } = "30year";
        public static List<string> AllTreasuryYields => new List<string>
        {
            Treasury3Month, Treasury2Year, Treasury5Year, Treasury7Year, Treasury10Year, Treasury30Year
        };
    }
}
