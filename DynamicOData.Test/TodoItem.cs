using DynamicOData.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

class TodoItem
{
    public string id { get; set; }
    public string text { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public bool deleted { get; set; }
    public string version { get; set; }

    public static Table Table
    {
        get
        {
            return new Table
            {
                Name = "TodoItem",
                Columns = Columns.Select(x => new Column { Name = x.Key, Type = x.Value })
            };
        }
    }

    public static Dictionary<string, Type> Columns = new Dictionary<string, Type>
    {
        { "id", typeof (String) },
        { "text", typeof (String) },
        { "createdAt", typeof (DateTimeOffset) },
        { "updatedAt", typeof (DateTimeOffset) },
        { "deleted", typeof (bool) },
        { "version", typeof (String) }
    };
}
