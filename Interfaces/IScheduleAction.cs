using Reminder.Models;

namespace Reminder.Interfaces
{
    public interface IScheduleAction
    {
        Task ExecuteAsync(Schedule schedule);
    }
}
