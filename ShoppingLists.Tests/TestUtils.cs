using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ShoppingLists.Core.Entities;
using System.IO;
using Dapper;
using LightInject;
using System.Reflection;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Core;

namespace ShoppingLists.Tests
{
    public static class TestUtils
    {
        private static ServiceContainer container = null;

        public static IDbConnection GetConnection()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ShoppingListsDbContext"].ConnectionString);
            con.Open();
            return con;
        }

        public static TResult InitialiseTestData<TResult>(string fileName)
        {
            string sql = File.ReadAllText(fileName);
            Enum.GetValues(typeof(Permissions)).Cast<Permissions>().ToList().ForEach(p => sql = sql.Replace("{{Permissions." + p.ToString() + "}}", ((int)p).ToString()));
            using (var con = TestUtils.GetConnection())
            {
                return con.Query<TResult>(sql).First();
            }
        }

        public static ServiceContainer GetDiContainer()
        {
            if (container == null)
            {
                container = new ServiceContainer();
                container.RegisterAssembly(Assembly.GetAssembly(typeof(DataAccessCompositionRoot)));
            }
            return container;
        }
    }
}
