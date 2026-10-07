using Reminder.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(Recipient recipient, string subject, string body);
    }
}
