using System;
using System.Collections.Generic;

namespace DynamicOData.Schema
{
    public class Table
    {
        public string Name { get; set; }
        public IEnumerable<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
