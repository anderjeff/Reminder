using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;
using Reminder.Models;
using Reminder.Interfaces;
using Reminder.Services;
using Reminder.Data;
using Reminder.Factories;
using Microsoft.Extensions.Configuration;

namespace Reminder;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        builder.Services.Configure<List<Schedule>>(builder.Configuration.GetSection("Schedules"));
        builder.Services.AddSingleton<Scheduler>();
        builder.Services.AddSingleton<INotificationService, EmailService>();
        builder.Services.AddSingleton<IGooglePhotosReminderDataAccess, GooglePhotosReminderDataAccess>();
        builder.Services.AddSingleton<ScheduleActionFactory>();

        // ensure we are using serilog.
        builder.Logging.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                "_logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .CreateLogger();

        builder.Services.AddSerilog();

        var app = builder.Build();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Reminder service starting...");

        var scheduler = app.Services.GetRequiredService<Scheduler>();

        try
        {
            while (true)
            {
                logger.LogInformation("Checking schedules...");
                await scheduler.CheckSchedulesAsync();
                await Task.Delay(5000);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while checking the schedule. Message: {Message}", ex.Message);
            throw; // note docker host will restart automatically.
        }        
    }
}