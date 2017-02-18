using System.Collections.Generic;
using System.Data.SQLite;

namespace DynamicOData.Test
{
    class Database
    {
        private static List<string> Commands = new List<string>
        {
            "DROP TABLE IF EXISTS TodoItem",
            "CREATE TABLE TodoItem (id TEXT, text TEXT, createdAt TEXT, updatedAt TEXT, deleted NUMBER, version TEXT)",
            "INSERT INTO TodoItem VALUES ('1', 'test', '2017-02-18T00:36:10.985Z', '2017-02-18T00:36:10.985Z', 1, '1')",
            "INSERT INTO TodoItem VALUES ('2', 'test', null, null, 0, null)",
            "INSERT INTO TodoItem VALUES ('3', 'test2', null, null, null, null)",
            "INSERT INTO TodoItem VALUES ('4', 'test3', null, null, null, null)"
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
