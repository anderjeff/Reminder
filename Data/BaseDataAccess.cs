using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Data
{
    public class BaseDataAccess
    {
        public BaseDataAccess()
        {
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS google_photos_reminder (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        sent_date DATETIME NOT NULL,
                        sent_to TEXT NOT NULL,
                        reminder_text TEXT NOT NULL,
                        start_search_date DATETIME NOT NULL,
                        end_search_date DATETIME NOT NULL
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE INDEX IF NOT EXISTS idx_google_photos_reminder_sent_to
                        ON google_photos_reminder(sent_to);
                    ";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE INDEX IF NOT EXISTS idx_google_photos_reminder_sent_date
                        ON google_photos_reminder(sent_date);
                    ";
                command.ExecuteNonQuery();
            }
        }

        public SqliteConnection GetConnection()
        {
            var connectionString = "Data Source=reminder.db";
            return new SqliteConnection(connectionString);
        }
    }
}
