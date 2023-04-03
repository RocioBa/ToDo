using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToDoApp.API.Middlewares;
using ToDoApp.API.Utils.Constants;
using ToDoApp.API.Utils.HealthChecks;
using ToDoApp.Data.Data.Context;

namespace ToDoApp.API.Infrastructure;

public static class ConfigureMiddlewares
{
    #region [- Methods -]

    #region [- ConfigureVersioningAndSwagger -]

    /// <summary>
    /// Adds versioning and swagger middlewares to web application pipeline
    /// </summary>
    private static void ConfigureVersioningAndSwagger(WebApplication app)
    {
        app.UseApiVersioning();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                APIDocument.APIVersions.ToList().ForEach(version =>
                {
                    p.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"v{version}");
                    p.RoutePrefix = string.Empty;
                });
            });
        }
    }
    #endregion

    #region [- ConfigureSqlDbMigration -]
    private static void ConfigureSqlDbMigration(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ToDoAppContext>().Database.Migrate();
        }
    }
    #endregion
     
    #region [- ConfigureAllMiddlewares -]
    public static WebApplication ConfigureAllMiddlewares(this WebApplication app)
    {
        app.UseIpRateLimiting();
        app.UseHttpLogging();
        app.UseErrorHandling();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors(CorsPolicies.Name);

        app.MapControllers();
        ConfigureVersioningAndSwagger(app);

        #region [- HealthChecks -]

        app.UseHealthChecks("/HC", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new HealthCheckingResponse
                {
                    GlobalStatus = report.Status.ToString(),
                    Components = report.Entries.Select(p => new AppComponent
                    {
                        Status = p.Value.Status.ToString(),
                        Component = p.Key.ToString(),
                        ErrorDescription = p.Value.Description
                    }),
                    TotalDuration = report.TotalDuration
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        });

        #endregion

        ConfigureSqlDbMigration(app);

        return app;
    }
    #endregion

    #endregion
}
