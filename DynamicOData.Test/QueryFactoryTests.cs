using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DynamicOData.Test
{
    [TestClass]
    public class QueryFactoryTests
    {
        [TestMethod]
        public void Generates_type()
        {
            var factory = new QueryFactory(TodoItem.Table);
            Assert.IsNotNull(factory.Type);
        }

        [TestMethod]
        public void Generates_queryables()
        {
            var factory = new QueryFactory(TodoItem.Table);
            Assert.IsInstanceOfType(factory.GetQuery(), typeof(IQueryable));
        }
    }
}
