using HealthCheck.Main.Interfaces;
using HealthCheck.Main.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace HealthCheck.Main.Checks
{
    public class ApiHealthCheck : ICustomHealthCheck
    {
        private readonly ApiHealthCheckOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiHealthCheck> _logger;

        public HealthCheckType Name => HealthCheckType.Api;

        public ApiHealthCheck(ApiHealthCheckOptions options, IHttpClientFactory httpClientFactory, ILogger<ApiHealthCheck> logger)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(_options.Endpoint, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("API is reachable.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("API returned error status.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API health check failed");
                return HealthCheckResult.Unhealthy("API check exception occurred.");
            }
        }
    }
}