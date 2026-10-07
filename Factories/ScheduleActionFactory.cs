using Microsoft.Extensions.DependencyInjection;
using Reminder.Interfaces;

namespace Reminder.Factories
{
    public class ScheduleActionFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScheduleActionFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IScheduleAction Create(string? typeName)
        {
            if(string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Invalid schedule action type name. Value cannot be null or whitespace.", nameof(typeName));
            }

            var type = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(a => a.GetType(typeName))
                .FirstOrDefault(t => t != null)
                ?? throw new InvalidOperationException($"Type '{typeName}' not found.");

            return (IScheduleAction)ActivatorUtilities.CreateInstance(
                _serviceProvider,
                type);
        }
    }
}
