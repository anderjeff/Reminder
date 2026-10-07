namespace Reminder.Interfaces
{
    public interface IGooglePhotosReminderDataAccess
    {
        void SaveReminder(string sentTo, string reminderText, DateTime startSearchDate, DateTime endSearchDate);
        DateTime GetLastSearchStartDate(string sentTo);
    }
}
