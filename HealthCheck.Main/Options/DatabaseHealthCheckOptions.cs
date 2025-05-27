using HealthCheck.Main.Checks;

namespace HealthCheck.Main.Options
{
    public class DatabaseHealthCheckOptions : HealthCheckOptionsBase
    {
        public string ConnectionString { get; set; }
        public DatabaseProvider Provider { get; set; }
    }
}