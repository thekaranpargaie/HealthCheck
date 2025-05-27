using HealthCheck.Main.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthCheck.Main.Services
{
    public class ScheduledHealthCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledHealthCheckService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public ScheduledHealthCheckService(IServiceProvider serviceProvider, ILogger<ScheduledHealthCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var compositeHealthCheck = scope.ServiceProvider.GetRequiredService<CompositeHealthCheck>();
                    await compositeHealthCheck.CheckHealthAsync(null, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Scheduled health check failed.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}