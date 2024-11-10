using System;
using System.Data.SQLite;
using System.IO;

namespace PotatoWPF.Models
{
    public class DBHelper
    {
        public string dbFilePath;

        // Constructor to initialize the database path
        public DBHelper()
        {
            string dbDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dbFilePath = Path.Combine(dbDirectory, "potato.db");
            InitializeDatabase();
        }

        // Create the database and table if they don't exist
        private void InitializeDatabase()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);

                using (var connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
                {
                    connection.Open();

                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS PotatoDBTable (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        TITLE TEXT,
                        TYPE TEXT,
                        VALUE DOUBLE,
                        ImageSource TEXT,
                        Deleteable INTEGER
                    );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string checkDataQuery = "SELECT COUNT(*) FROM PotatoDBTable;";
                    using (var command = new SQLiteCommand(checkDataQuery, connection))
                    {
                        long count = (long)command.ExecuteScalar();

                        // Only insert initial data if the table is empty
                        if (count == 0)
                        {
                            string insertDataQuery = @"
                            INSERT INTO PotatoDBTable (TITLE, TYPE, VALUE, ImageSource, Deleteable) VALUES
                            (@type, @color, @value, @ImageSource, 1);";

                            using (var insertCommand = new SQLiteCommand(insertDataQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@type", "A");
                                insertCommand.Parameters.AddWithValue("@color", "yellow");
                                insertCommand.Parameters.AddWithValue("@value", 100);
                                insertCommand.Parameters.AddWithValue("@ImageSource", 100);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
