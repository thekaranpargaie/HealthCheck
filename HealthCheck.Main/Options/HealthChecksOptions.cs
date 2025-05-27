using System.Collections.Generic;

namespace HealthCheck.Main.Options
{
    public class HealthChecksOptions
    {
        public List<DatabaseHealthCheckOptions> Databases { get; set; } = new();
        public List<ApiHealthCheckOptions> Apis { get; set; } = new();
        public List<DiskSpaceHealthCheckOptions> DiskSpaces { get; set; } = new();
    }
}