using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.BusinessLayer;

namespace ShoppingLists.Tests.BusinessLayer
{
    [TestClass]
    public class StringComparisonToolsTests
    {
        [TestMethod]
        public void PadNumbers_ShouldAllowAlphaNumericStringsToBeSorted_WhenUsedAsTheKeySelectorInALinqOrderBy()
        {
            var strings = new List<string>()
            {
                "Test !",
                "Test 2",
                "Test 1",
                "Test a",
                "Test 3",
                "Test 4",
                null,
                "Test 10",
                "Test z",
                null
            };
            var orderedStrings = strings.OrderBy(s => StringComparisonTools.PadNumbers(s)).ToList();

            int i = 0;
            Assert.AreEqual(null, orderedStrings[i++]);
            Assert.AreEqual(null, orderedStrings[i++]);
            Assert.AreEqual("Test !", orderedStrings[i++]);
            Assert.AreEqual("Test 1", orderedStrings[i++]);
            Assert.AreEqual("Test 2", orderedStrings[i++]);
            Assert.AreEqual("Test 3", orderedStrings[i++]);
            Assert.AreEqual("Test 4", orderedStrings[i++]);
            Assert.AreEqual("Test 10", orderedStrings[i++]);
            Assert.AreEqual("Test a", orderedStrings[i++]);
            Assert.AreEqual("Test z", orderedStrings[i++]);
        }
    }
}
