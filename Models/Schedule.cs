using System.Text.Json.Serialization;

namespace Reminder.Models
{
    public class Schedule
    {
        public Schedule()
        {
        }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("cron")]
        public string? Cron { get; set; }

        [JsonPropertyName("recipients")]
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        public string? Action { get; set; }
    }
}
