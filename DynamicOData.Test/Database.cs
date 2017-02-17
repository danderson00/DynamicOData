using System.Collections.Generic;
using System.Data.SQLite;

namespace DynamicOData.Test
{
    class Database
    {
        private static List<string> Commands = new List<string>
        {
            "DROP TABLE IF EXISTS TodoItem",
            "CREATE TABLE TodoItem (id TEXT, text TEXT)",
            "INSERT INTO TodoItem VALUES ('1', 'test')",
            "INSERT INTO TodoItem VALUES ('2', 'test')",
            "INSERT INTO TodoItem VALUES ('3', 'test2')"
        };

        public static void Initialize()
        {
            var connection = new SQLiteConnection("data source=test.sqlite");
            connection.Open();
            Commands.ForEach(sql => new SQLiteCommand(sql, connection).ExecuteNonQuery() );
            connection.Close();
        }
    }
}
