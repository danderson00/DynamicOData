using Microsoft.CSharp;
using DynamicOData.Schema;
using System;
using System.CodeDom.Compiler;
using System.Data.Entity;
using System.Linq;
using System.Data.Common;

namespace DynamicOData
{
    public class QueryFactory : IDisposable
    {
        private DbContext Context { get; set; }
        public Type Type { get; set; }

        public QueryFactory(Table table, string connectionString = "Data Source=localhost")
        {
            var results = this.Compile(table);
            var assembly = results.CompiledAssembly;
            this.Context = (DbContext)Activator.CreateInstance(assembly.GetType("TargetContext"), connectionString);
            this.Type = assembly.GetType(table.Name);
        }

        public QueryFactory(Table table, DbConnection existingConnection, bool factoryOwnsConnection = false)
        {
            var results = this.Compile(table);
            var assembly = results.CompiledAssembly;
            this.Context = (DbContext)Activator.CreateInstance(assembly.GetType("TargetContext"), existingConnection, factoryOwnsConnection);
            this.Type = assembly.GetType(table.Name);
        }

        private CompilerResults Compile(Table table)
        {
            var code = ClassFactory.Generate(table);
            var codeProvider = new CSharpCodeProvider();

            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("EntityFramework.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");

            var results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                throw new Exception($"Failed to compile: {results.Errors[0].ErrorText}");
            }

            return results;
        }

        public IQueryable GetQuery()
        {
            return (IQueryable)this.Context.GetType().GetProperty("Target").GetValue(this.Context);
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
