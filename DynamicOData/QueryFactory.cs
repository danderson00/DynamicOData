using Microsoft.CSharp;
using DynamicOData.Schema;
using System;
using System.CodeDom.Compiler;
using System.Data.Entity;
using System.Linq;

namespace DynamicOData
{
    public class QueryFactory
    {
        private DbContext Context { get; set; }
        public Type Type { get; set; }

        public QueryFactory(Table table, string connectionString = "Data Source=localhost")
        {
            var code = ClassFactory.Generate(table);
            var codeProvider = new CSharpCodeProvider();

            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("EntityFramework.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");

            var results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if(results.Errors.Count > 0)
            {
                throw new Exception($"Failed to compile: {results.Errors[0].ErrorText}");
            }

            var assembly = results.CompiledAssembly;
            this.Context = (DbContext)Activator.CreateInstance(assembly.GetType("TargetContext"), connectionString);
            this.Type = assembly.GetType(table.Name);
        }

        public IQueryable GetQuery()
        {
            return (IQueryable)this.Context.GetType().GetProperty("Target").GetValue(this.Context);
        }
    }
}
