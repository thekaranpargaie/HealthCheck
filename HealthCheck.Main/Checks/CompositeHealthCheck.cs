using HealthCheck.Main.Options;
using HealthCheck.Main.Models;
using HealthCheck.Main.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HealthCheck.Main.Checks
{
    public class CompositeHealthCheck : IHealthCheck
    {
        private readonly HealthChecksOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CompositeHealthCheck> _logger;
        private readonly IHealthCheckHistoryStore _historyStore;

        public CompositeHealthCheck(
            IOptions<HealthChecksOptions> options,
            IServiceProvider serviceProvider,
            ILogger<CompositeHealthCheck> logger,
            IHealthCheckHistoryStore historyStore)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _historyStore = historyStore;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var results = new Dictionary<string, HealthReportEntry>();
            var now = DateTime.UtcNow;

            // Database checks
            foreach (var dbOpt in _options.Databases.Where(x => x.IsEnabled))
            {
                var check = new DatabaseHealthCheck(dbOpt, _serviceProvider.GetService(typeof(ILogger<DatabaseHealthCheck>)) as ILogger<DatabaseHealthCheck>);
                var result = await check.CheckHealthAsync(context, cancellationToken);
                var key = dbOpt.Key;
                results.Add(key, new HealthReportEntry(result.Status, result.Description));
                _historyStore.AddPing(new HealthCheckPingResult
                {
                    Timestamp = now,
                    ServiceKey = key,
                    IsHealthy = result.Status == HealthStatus.Healthy,
                    Description = result.Description
                });
            }

            // API checks
            foreach (var apiOpt in _options.Apis.Where(x => x.IsEnabled))
            {
                var httpClientFactory = _serviceProvider.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
                var check = new ApiHealthCheck(apiOpt, httpClientFactory, _serviceProvider.GetService(typeof(ILogger<ApiHealthCheck>)) as ILogger<ApiHealthCheck>);
                var result = await check.CheckHealthAsync(context, cancellationToken);
                var key = apiOpt.Key;
                results.Add(key, new HealthReportEntry(result.Status, result.Description));
                _historyStore.AddPing(new HealthCheckPingResult
                {
                    Timestamp = now,
                    ServiceKey = key,
                    IsHealthy = result.Status == HealthStatus.Healthy,
                    Description = result.Description
                });
            }

            // DiskSpace checks
            foreach (var diskOpt in _options.DiskSpaces.Where(x => x.IsEnabled))
            {
                var check = new DiskSpaceHealthCheck(diskOpt, _serviceProvider.GetService(typeof(ILogger<DiskSpaceHealthCheck>)) as ILogger<DiskSpaceHealthCheck>);
                var result = await check.CheckHealthAsync(context, cancellationToken);
                var key = diskOpt.Key;
                results.Add(key, new HealthReportEntry(result.Status, result.Description));
                _historyStore.AddPing(new HealthCheckPingResult
                {
                    Timestamp = now,
                    ServiceKey = key,
                    IsHealthy = result.Status == HealthStatus.Healthy,
                    Description = result.Description
                });
            }

            // Aggregate
            var unhealthy = results.FirstOrDefault(r => r.Value.Status == HealthStatus.Unhealthy);
            if (!unhealthy.Equals(default(KeyValuePair<string, HealthReportEntry>)))
            {
                return HealthCheckResult.Unhealthy("One or more health checks failed.", data: results.ToDictionary(r => r.Key, r => (object)r.Value.Description));
            }

            return HealthCheckResult.Healthy("All health checks passed.", data: results.ToDictionary(r => r.Key, r => (object)r.Value.Description));
        }

        private class HealthReportEntry
        {
            public HealthStatus Status { get; }
            public string Description { get; }

            public HealthReportEntry(HealthStatus status, string description)
            {
                Status = status;
                Description = description;
            }
        }
    }
}