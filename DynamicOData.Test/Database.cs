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
            $"INSERT INTO TodoItem VALUES ('1', 'test', '{System.DateTime.Parse("2020-01-01T00:00:00.000Z").ToString("o")}', '2017-02-18T00:36:10.985Z', 1, '1')",
            $"INSERT INTO TodoItem VALUES ('2', 'test', '{System.DateTime.Parse("2020-01-02T00:00:00.000Z").ToString("o")}', null, 0, null)",
            $"INSERT INTO TodoItem VALUES ('3', 'test2', '{System.DateTime.Parse("2020-01-03T00:00:00.000Z").ToString("o")}', null, null, null)",
            $"INSERT INTO TodoItem VALUES ('4', 'test3', '{System.DateTime.Parse("2020-01-04T00:00:00.000Z").ToString("o")}', null, null, null)"
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
