using HealthCheck.Main.Checks;

namespace HealthCheck.Main.Options
{
    public class ApiHealthCheckOptions : HealthCheckOptionsBase
    {
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }
    }
}