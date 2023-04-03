using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using ToDoApp.Data.Data.Context;

namespace ToDoApp.API.Utils.HealthChecks;

public class SqlHealthChecking : IHealthCheck
{
    #region [- Constructor -]

    private readonly ToDoAppContext _context;
    public SqlHealthChecking(ToDoAppContext context)
    {
        _context = context;  
    }
    #endregion

    #region [- Methods -]

    #region [- CheckHealthAsync -]
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if(!_context.Database.CanConnect())
                return Task.FromResult(HealthCheckResult.Unhealthy("Sql database connection failed"));

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
