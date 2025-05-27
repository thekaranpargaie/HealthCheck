using HealthCheck.Main.Interfaces;
using HealthCheck.Main.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.Common;

namespace HealthCheck.Main.Checks
{
    public class DatabaseHealthCheck : ICustomHealthCheck
    {
        private readonly ILogger<DatabaseHealthCheck> _logger;
        private readonly DatabaseHealthCheckOptions _options;

        public HealthCheckType Name => HealthCheckType.Database;

        public DatabaseHealthCheck(DatabaseHealthCheckOptions options, ILogger<DatabaseHealthCheck> logger)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection(_options.Provider, _options.ConnectionString);
                await connection.OpenAsync(cancellationToken);

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy("Database connection successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return HealthCheckResult.Unhealthy("Database connection failed.");
            }
        }

        private static DbConnection CreateConnection(DatabaseProvider provider, string connectionString)
        {
            return provider switch
            {
                DatabaseProvider.MSSQL => new SqlConnection(connectionString),
                DatabaseProvider.MySQL => new MySqlConnection(connectionString),
                DatabaseProvider.PostgreSQL => new NpgsqlConnection(connectionString),
                _ => throw new NotSupportedException($"Unsupported database provider: {provider}")
            };
        }
    }
}