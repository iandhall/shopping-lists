using System;
using System.IO;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.Tests
{
    [TestClass]
    public class TestsStartup
    {
        [AssemblyInitialize]
        public static void Initialise(TestContext testContext)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            DataAccessStartup.Initialise();
            using (var con = TestUtils.GetConnection())
            {
                con.Execute(File.ReadAllText(@"DataAccessLayer\sql\WipeExistingData.sql"));
            }
        }
    }
}
