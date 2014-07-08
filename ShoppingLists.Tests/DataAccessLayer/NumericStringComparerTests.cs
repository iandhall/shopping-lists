using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.Tests.DataAccess
{
    [TestClass]
    public class NumericStringComparerTests
    {
        [TestMethod]
        public void TestCompare()
        {
            var strings = new List<string>()
            {
                "!",
                "2",
                "1",
                "a",
                "3",
                "4",
                null,
                "10",
                "z",
                null
            };
            var orderedStrings = strings.OrderByDescending(s => s, new NumericStringComparer()).ToList();
            int i = 0;
            Assert.AreEqual("10", orderedStrings[i++]);
            Assert.AreEqual("4", orderedStrings[i++]);
            Assert.AreEqual("3", orderedStrings[i++]);
            Assert.AreEqual("2", orderedStrings[i++]);
            Assert.AreEqual("1", orderedStrings[i++]);
            Assert.AreEqual("z", orderedStrings[i++]);
            Assert.AreEqual("a", orderedStrings[i++]);
            Assert.AreEqual("!", orderedStrings[i++]);
            Assert.AreEqual(null, orderedStrings[i++]);
            Assert.AreEqual(null, orderedStrings[i++]);
        }
    }
}
