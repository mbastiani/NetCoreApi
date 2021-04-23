using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreApi.HealthChecks
{
    public class PrimeiroHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            bool saudavel = true;

            if (saudavel)
                return HealthCheckResult.Healthy($"{nameof(PrimeiroHealthCheck)} está online");

            return HealthCheckResult.Unhealthy($"{nameof(PrimeiroHealthCheck)} está offline");
        }
    }
}
