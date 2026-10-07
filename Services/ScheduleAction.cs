using Reminder.Interfaces;
using Reminder.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Services
{
    public class ScheduleAction : IScheduleAction
    {
        protected readonly INotificationService _emailService;

        public ScheduleAction(INotificationService emailService)
        {
            _emailService = emailService;
        }

        public virtual Task ExecuteAsync(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
