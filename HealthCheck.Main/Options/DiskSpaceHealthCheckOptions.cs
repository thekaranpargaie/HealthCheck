using HealthCheck.Main.Checks;

namespace HealthCheck.Main.Options
{
    public class DiskSpaceHealthCheckOptions : HealthCheckOptionsBase
    {
        public string RemoteServer { get; set; }
        public string Credentials { get; set; }
        public string DriveLetter { get; set; }
    }
}