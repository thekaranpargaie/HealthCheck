using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthCheck.Main.Checks;

namespace HealthCheck.Main.Interfaces
{
    public interface ICustomHealthCheck : IHealthCheck
    {
        HealthCheckType Name { get; }
    }
}