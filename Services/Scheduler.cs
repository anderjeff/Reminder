using Cronos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reminder.Factories;
using Reminder.Models;

namespace Reminder.Services
{
    public class Scheduler
    {
        private IOptions<List<Schedule>> _schedules;
        private DateTime _lastCheckTime = DateTime.UtcNow;
        private readonly ILogger<Scheduler> _logger;
        private readonly ScheduleActionFactory _scheduleActionFactory;

        public Scheduler(IOptions<List<Schedule>> schedules, ILogger<Scheduler> logger, ScheduleActionFactory scheduleActionFactory)
        {
            _schedules = schedules;
            _logger = logger;
            _scheduleActionFactory = scheduleActionFactory;
        }

        public async Task CheckSchedulesAsync()
        {
            var now = DateTime.UtcNow;

            foreach (var schedule in _schedules.Value)
            {
                if (string.IsNullOrWhiteSpace(schedule.Cron))
                {
                    _logger.LogWarning("Schedule '{ScheduleName}' has an empty or null Cron expression. Skipping execution.", schedule.Name);
                    continue;
                }
                if (!schedule.Enabled == true)
                {
                    _logger.LogInformation("Schedule '{ScheduleName}' is disabled. Skipping execution.", schedule.Name);
                    continue;
                }

                var cron = CronExpression.Parse(schedule.Cron);
                var next = cron.GetNextOccurrence(_lastCheckTime);

                _logger.LogInformation("Next scheduled occurrence is {next:g}", next);
                _logger.LogInformation("Last check time is {last:g}", _lastCheckTime);

                if (next.HasValue)
                {
                    // execute the action if the next occurrence is within the last check time and now
                    if(next.Value > _lastCheckTime && next.Value <= now) 
                    {
                        var scheduleAction = _scheduleActionFactory.Create(schedule.Action);
                        await scheduleAction.ExecuteAsync(schedule);
                        _logger.LogInformation("Executed schedule '{ScheduleName}' at {ExecutionTime} with action {ActionName}.", schedule.Name, now, scheduleAction.GetType().Name);
                    }
                }
            }

            _lastCheckTime = now;
        }
    }
}
