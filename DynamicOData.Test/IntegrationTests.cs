using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DynamicOData.Test
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Generates_simple_sql()
        {
            var factory = new QueryFactory(TodoItem.Table);
            var filter = new ODataFilter(TodoItem.Table, factory.Type);
            var query = factory.GetQuery();
            query = filter.ApplyTo(query, "id eq '1'");

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
            // the TodoItem type used here is not the same as the one returned by the IQueryable - anything other than Count() will fail
            Assert.AreEqual(1, host.GetQuery("id eq '1'").OfType<TodoItem>().Count());
            Assert.AreEqual(2, host.GetQuery("text eq 'test'").OfType<TodoItem>().Count());
        }
    }
}


