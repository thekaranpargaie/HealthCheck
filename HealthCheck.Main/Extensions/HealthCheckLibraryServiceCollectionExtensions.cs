using HealthCheck.Main.Checks;
using HealthCheck.Main.Exceptions;
using HealthCheck.Main.Options;
using HealthCheck.Main.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace HealthCheck.Main.Extensions
{
    public static class HealthCheckLibraryServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all required health check services for the HealthCheck.Main.
        /// </summary>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services, configuration);
            ValidateUniqueKeys(configuration);
            RegisterCoreServices(services);
            return services;
        }

        private static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
        {
			services.Configure<HealthChecksOptions>(configuration.GetSection("HealthChecks"));
		}

        private static void ValidateUniqueKeys(IConfiguration configuration)
        {
			var healthChecksOptions = new HealthChecksOptions();
			configuration.GetSection("HealthChecks").Bind(healthChecksOptions);
			var allKeys = GetAllKeys(healthChecksOptions);

            var duplicateKey = allKeys.GroupBy(k => k)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key)
                                      .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(duplicateKey))
            {
                throw new DuplicateHealthCheckKeyException($"Duplicate health check key found: '{duplicateKey}'. Each health check must have a unique key.");
            }
        }

        private static List<string> GetAllKeys(HealthChecksOptions options)
        {
            return options.Databases.Select(d => d.Key)
                .Concat(options.Apis.Select(a => a.Key))
                .Concat(options.DiskSpaces.Select(d => d.Key))
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .ToList();
        }

        private static void RegisterCoreServices(IServiceCollection services)
        {
            services.AddSingleton<IHealthCheckHistoryStore, HealthCheckHistoryStore>();
            services.AddSingleton<CompositeHealthCheck>();
            services.AddHostedService<ScheduledHealthCheckService>();
            services.AddHttpClient();
        }
    }
}