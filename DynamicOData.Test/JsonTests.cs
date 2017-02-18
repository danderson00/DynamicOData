using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DynamicOData.Test
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        public void Queryable_can_be_serialized_to_JSON()
        {
            Database.Initialize();
            var host = new QueryHost("TodoItem", TodoItem.Columns, "sqlite");

            var query = host.GetQuery("text eq 'test'");
            var json = JsonConvert.SerializeObject(query);

            Assert.AreEqual(@"[{""id"":""1"",""text"":""test"",""createdAt"":""2017-02-17T16:36:10.985"",""updatedAt"":""2017-02-17T16:36:10.985"",""deleted"":true,""version"":""1""},{""id"":""2"",""text"":""test"",""createdAt"":null,""updatedAt"":null,""deleted"":false,""version"":null}]", json);
        }
    }
}
