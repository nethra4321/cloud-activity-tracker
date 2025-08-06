using System;

namespace backend.Models
{
    public class ActivityEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
       public string UserId { get; set; } = string.Empty;
    }
}
