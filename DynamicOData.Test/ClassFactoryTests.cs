using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicOData.Test
{
    [TestClass]
    public class ClassFactoryTests
    {
        [TestMethod]
        public void Generates_simple_class()
        {
            var code = ClassFactory.Generate(TodoItem.Table);
            var expected = @"
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

public class TodoItem
{
    public String id { get; set; }
    public String text { get; set; }
}

public class TargetContext : DbContext
{
    public TargetContext(string connectionString) : base(connectionString) { }
    public DbSet<TodoItem> Target { get; set; }

    protected override void OnModelCreating(DbModelBuilder model)
    {
        Database.SetInitializer<TargetContext>(null);
        model.Conventions.Remove<PluralizingTableNameConvention>();
        base.OnModelCreating(model);
    }
}";
            Assert.AreEqual(expected, code);
        }
    }
}
