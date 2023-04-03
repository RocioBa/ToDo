using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace ToDoApp.API.Utils.HealthChecks;

public class RedisHealthChecking : IHealthCheck
{
    #region [- Constructor -]

    private readonly IConnectionMultiplexer _redis;
    public RedisHealthChecking(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    #endregion

    #region [- Methods -]

    #region [- CheckHealthAsync -]
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {     
        try
        {
            var db = _redis.GetDatabase();
            db.StringGet("Item");
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
        }
    }
    #endregion

    #endregion
}
