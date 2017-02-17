﻿using DynamicOData.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicOData
{
    public class QueryHost
    {
        private QueryFactory Queries;
        private ODataFilter Filter;

        public QueryHost(string tableName, Dictionary<string, Type> columns, string connectionStringOrName)
            : this(new Table
            {
                Name = tableName,
                Columns = columns.Select(column => new Column { Name = column.Key, Type = column.Value })
            }, connectionStringOrName)
        { }

        public QueryHost(Table table, string connectionStringOrName = null)
        {
            Queries = new QueryFactory(table, connectionStringOrName);
            Filter = new ODataFilter(table, Queries.Type);
        }

        public IQueryable GetQuery(string odata)
        {
            return Filter.ApplyTo(Queries.GetQuery(), odata);
        }

        public IQueryable GetQuery()
        {
            return Queries.GetQuery();
        }
    }
}