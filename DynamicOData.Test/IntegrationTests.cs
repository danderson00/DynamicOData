using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Data.SQLite;
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
    [Extent1].[text] AS [text], 
    [Extent1].[createdAt] AS [createdAt], 
    [Extent1].[updatedAt] AS [updatedAt], 
    [Extent1].[deleted] AS [deleted], 
    [Extent1].[version] AS [version]
    FROM [dbo].[TodoItem] AS [Extent1]
    WHERE [Extent1].[id] = @p__linq__0";
            Assert.AreEqual(expected, query.ToString());
        }

        [TestMethod]
        public void Queries_SQLite_database()
        {
            Database.Initialize();

            var host = new QueryHost("TodoItem", TodoItem.Columns, "sqlite");
            Assert.AreEqual(1, host.GetQuery("id eq '1'").ToListAsync().Result.Count());
            Assert.AreEqual(2, host.GetQuery("text eq 'test'").ToListAsync().Result.Count());
        }

        [TestMethod]
        public void Queries_SQLite_database_with_existing_connection()
        {
            Database.Initialize();

            var connection = new SQLiteConnection("data source=test.sqlite");
            var host = new QueryHost("TodoItem", TodoItem.Columns, connection, true);
            Assert.AreEqual(1, host.GetQuery("id eq '1'").ToListAsync().Result.Count());
            Assert.AreEqual(2, host.GetQuery("text eq 'test'").ToListAsync().Result.Count());
            // dates in SQLite are screwy. They're represented as strings and comparisons don't work quite right. Works fine in SQL Server.
            Assert.AreEqual(3, host.GetQuery($"createdAt gt datetime'2020-01-01T00:00:00.000Z'").ToListAsync().Result.Count());
            Assert.AreEqual(2, host.GetQuery($"createdAt lt datetime'{DateTime.Parse("2020-01-03T00:00:00.000Z").ToString("o")}'").ToListAsync().Result.Count());
        }
    }
}


