# DynamicOData

This library converts OData strings into IQueryable objects representing a
query against a remote database. 

The query generator requires an initial schema expressed as an 
`IDictionary<string, Type>` where each key value pair represents the name and
type of a column in the database table.

## Usage

The `DynamicOData.QueryHost` object encapsulates functionality.

	public QueryHost(string tableName, Dictionary<string, Type> columns, string connectionStringOrName)

`QueryHost` exposes a single function:

	public IQueryable GetQuery(string odata)

## Example

    var host = new QueryHost("TodoItem", new Dictionary<string, Type>
        {
            { "id", typeof (String) },
            { "text", typeof (String) },
            { "createdAt", typeof (DateTimeOffset) },
            { "updatedAt", typeof (DateTimeOffset) },
            { "deleted", typeof (bool) },
            { "version", typeof (Byte[]) }
        }, "connectionStringOrName");
    var todoItems = await host.GetQuery("createdAt gt datetime'2020-01-01T00:00:00.000Z' and deleted eq false").ToListAsync();
