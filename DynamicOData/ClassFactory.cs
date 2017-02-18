using DynamicOData.Schema;
using System;
using System.Linq;

namespace DynamicOData
{
    public class ClassFactory
    {
        public static string Generate(Table table)
        {
            return string.Format(@"
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

public class {0}
{{
{1}
}}

public class TargetContext : DbContext
{{
    public TargetContext(string connectionString) : base(connectionString) {{ }}
    public TargetContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) {{ }}
    public DbSet<{0}> Target {{ get; set; }}

    protected override void OnModelCreating(DbModelBuilder model)
    {{
        Database.SetInitializer<TargetContext>(null);
        model.Conventions.Remove<PluralizingTableNameConvention>();
        base.OnModelCreating(model);
    }}
}}", table.Name, string.Join("\r\n", table.Columns.Select(ColumnSource)));
        }

        private static string ColumnSource(Column column)
        {
            return $"    public {GetColumnTypeName(column.Type)} {column.Name} {{ get; set; }}";
        }

        private static string GetColumnTypeName(Type type)
        {
            // some data sources don't support DateTimeOffset. Requires unit tests to ensure integrity
            if (type.Name == "DateTimeOffset")
                type = typeof(DateTime);

            var nullableModifier = type.IsValueType ? "?" : "";

            return type.Name + nullableModifier;
        }
    }
}
