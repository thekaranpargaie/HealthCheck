using HealthCheck.Main.Checks;

namespace HealthCheck.Main.Options
{
    public abstract class HealthCheckOptionsBase
    {
        public bool IsEnabled { get; set; }
        public HealthCheckType Type { get; set; }
        public string Key { get; set; } // Unique key for each health check
    }
}