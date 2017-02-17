using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DynamicOData.Test
{
    [TestClass]
    public class ODataTests
    {
        [TestMethod]
        public void Filters_arrays()
        {
            var odata = new ODataFilter(TodoItem.Table, typeof(TodoItem));
            var source = new TodoItem[]
            {
                new TodoItem { id = "1", text = "test" },
                new TodoItem { id = "2", text = "test" },
                new TodoItem { id = "3", text = "test2" },
            }.AsQueryable();

            Assert.AreEqual(1, odata.ApplyTo(source, "id eq '1'").OfType<TodoItem>().Count());
            Assert.AreEqual(2, odata.ApplyTo(source, "text eq 'test'").OfType<TodoItem>().Count());
        }
    }
}