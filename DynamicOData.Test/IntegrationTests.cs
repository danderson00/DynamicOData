using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace DynamicOData.Test
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Generates_simple_SQL()
        {
            var factory = new QueryFactory(TodoItem.Table);
            var filter = new ODataFilter(TodoItem.Table, factory.Type);
            var query = filter.ApplyTo(factory.GetQuery(), "id eq '1'");

            var expected = @"SELECT 
    [Extent1].[id] AS [id], 
    [Extent1].[text] AS [text]
    FROM [dbo].[TodoItem] AS [Extent1]
    WHERE [Extent1].[id] = @p__linq__0";
            Assert.AreEqual(expected, query.ToString());
        }

        [TestMethod]
        public void Queries_SQLite_database()
        {
            Database.Initialize();

            var host = new QueryHost("TodoItem", TodoItem.Columns, "sqlite");
            Assert.AreEqual(2, host.GetQuery("text eq 'test'").ToListAsync().Result.Count());
            Assert.AreEqual(2, host.GetQuery("text eq 'test'").ToListAsync().Result.Count());
        }
    }
}


