using DynamicOData.Schema;
using System;
using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace DynamicOData
{
    public class ODataFilter
    {
        private ODataQueryContext Context;

        public ODataFilter(Table table, Type type)
        {
            this.Context = new ODataQueryContext(EdmModelFactory.Generate(table), type);
        }

        public IQueryable ApplyTo(IQueryable query, string odata) { 
            var filter = new FilterQueryOption(odata, this.Context);
            return filter.ApplyTo(query, new ODataQuerySettings());
        }
    }
}
