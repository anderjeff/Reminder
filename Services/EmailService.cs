using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Reminder.Interfaces;
using Reminder.Models;

namespace Reminder.Services
{
    public class EmailService : INotificationService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendAsync(Recipient recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jeff", "anderson.jjames@gmail.com"));
            message.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
            if (!recipient.Email.StartsWith("414"))
            { 
                message.Subject = subject;
            }
            message.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587);
            await client.AuthenticateAsync("anderson.jjames@gmail.com", "esap degm cswe muzo");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
