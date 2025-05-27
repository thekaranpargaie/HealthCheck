using System;

namespace HealthCheck.Main.Models
{
    public class HealthCheckPingResult
    {
        public DateTime Timestamp { get; set; }
        public string ServiceKey { get; set; }
        public bool IsHealthy { get; set; }
        public string Description { get; set; }
    }
}