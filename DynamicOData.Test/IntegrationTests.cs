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
            AssertCount(host.GetQuery("id eq '1'"), 1);
            AssertCount(host.GetQuery("text eq 'test'"), 2);
        }

        private void AssertCount(IQueryable query, int expected)
        {
            // this is interesting - the types aren't the same (the whole idea is to not have a defined type), but count still works
            var c = query.OfType<TodoItem>().Count();
            Assert.AreEqual(expected, c);

            // a more reliable way of counting in the absence of a concrete type
            var count = 0;
            foreach (var item in query)
            {
                count++;
            }
            Assert.AreEqual(expected, count);
        }
    }
}


