using DynamicOData.Schema;
using System;
using System.Collections.Generic;

class TodoItem
{
    public string id { get; set; }
    public string text { get; set; }

    public static Table Table
    {
        get
        {
            return new Table
            {
                Name = "TodoItem",
                Columns = new Column[]
                {
                    new Column { Name = "id", Type = typeof (String) },
                    new Column { Name = "text", Type = typeof (String) }
                }
            };
        }
    }

    public static Dictionary<string, Type> Columns = new Dictionary<string, Type>
    {
        { "id", typeof (String) },
        { "text", typeof (String) },
        { "createdAt", typeof (DateTime) },
        { "updatedAt", typeof (DateTime) },
        { "deleted", typeof (bool) },
        { "version", typeof (String) }
    };
}
