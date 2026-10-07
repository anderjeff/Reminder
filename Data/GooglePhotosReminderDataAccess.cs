using Reminder.Interfaces;

namespace Reminder.Data
{
    public class GooglePhotosReminderDataAccess : BaseDataAccess, IGooglePhotosReminderDataAccess
    {
        private DateTime _defaultStartSearchDate = new DateTime(2012, 4, 17);

        public void SaveReminder(string sentTo, string reminderText, DateTime startSearchDate, DateTime endSearchDate)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO google_photos_reminder (sent_date, sent_to, reminder_text, start_search_date, end_search_date)
                    VALUES ($sentDate, $sentTo, $reminderText, $startSearchDate, $endSearchDate);
                ";
                command.Parameters.AddWithValue("$sentDate", DateTime.Now);
                command.Parameters.AddWithValue("$sentTo", sentTo);
                command.Parameters.AddWithValue("$reminderText", reminderText);
                command.Parameters.AddWithValue("$startSearchDate", startSearchDate);
                command.Parameters.AddWithValue("$endSearchDate", endSearchDate);
                command.ExecuteNonQuery();
            }
        }

        public DateTime GetLastSearchStartDate(string sentTo)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT end_search_date FROM google_photos_reminder WHERE sent_to = $sentTo ORDER BY end_search_date DESC LIMIT 1;
                ";
                command.Parameters.AddWithValue("$sentTo", sentTo);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var startSearchDate = reader.GetDateTime(0);
                        return startSearchDate;
                    }
                    else
                    {
                        return _defaultStartSearchDate;
                    }
                }
            }
        }
    }
}
