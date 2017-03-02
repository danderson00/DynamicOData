# DynamicOData

This library converts OData filter queries into IQueryable objects that encapsulate
a query against a remote database, without requiring compiled C# classes. 

The query generator requires an initial schema expressed as an 
`IDictionary<string, Type>` where each key value pair represents the name and
type of a column in the database table.

## Installation

	Install-Package DynamicOData

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

    List<object> todoItems = await host
		.GetQuery("createdAt gt datetime'2017-01-01T00:00:00.000Z' and deleted eq false")
		.ToListAsync();

## How Does it Work?

DynamicOData generates and compiles a C# POCO and Entity Framework DbContext
class from the supplied metadata and uses this to generate an IQueryable 
against any data source supported by Entity Framework. 

It also generates an EdmModel class that is used to parse supplied OData queries. 
These filters are applied to IQueryable objects generated by Entity Framework
and returned.

**Creation of QueryHost objects is an expensive operation and instances
should be reused wherever possible.**