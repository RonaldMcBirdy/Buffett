using Buffett.Endpoint.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Buffett.UnitTests.Helpers
{
    [TestFixture]
    public class SortedDateListHelperTests
    {
        private List<DateTime> _dates;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dates = new List<DateTime>
            {
                DateTime.Now,
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(-3),
                DateTime.Now.AddDays(-4)
            };
        }

        [Test] 
        public void SortedDateListHelperTest_ToDate()
        {
            var target = DateTime.Now.AddDays(-2);
            var index = SortedDateListHelper.FindIndex(_dates, target, true);
            Assert.AreEqual(2, index);

            target = DateTime.Now.AddDays(4);
            index = SortedDateListHelper.FindIndex(_dates, target, true);
            Assert.AreEqual(0, index);

            target = DateTime.Now.AddDays(-5);
            Assert.Throws<Exception>(() => SortedDateListHelper.FindIndex(_dates, target, true));
        }

        [Test]
        public void SortedDateListHelperTest_FromDate()
        {
            var target = DateTime.Now.AddDays(-2);
            var index = SortedDateListHelper.FindIndex(_dates, target, false);
            Assert.AreEqual(2, index);

            target = DateTime.Now.AddDays(4);
            index = SortedDateListHelper.FindIndex(_dates, target, false);
            Assert.AreEqual(0, index);

            target = DateTime.Now.AddDays(-5);
            index = SortedDateListHelper.FindIndex(_dates, target, false);
            Assert.AreEqual(3, index);
        }
    }
}
