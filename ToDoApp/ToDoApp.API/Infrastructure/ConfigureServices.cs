
using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToDoApp.API.Services;
using ToDoApp.API.Utils.ConfigOptions;
using ToDoApp.API.Utils.Constants;
using ToDoApp.API.Utils.HealthChecks;
using ToDoApp.API.Utils.Swagger;
using ToDoApp.Data.Infrastructure;
using static ToDoApp.API.Utils.Swagger.SwaggerHelper;

namespace ToDoApp.API.Infrastructure;
public static class ConfigureServices
{
    #region [- Methods -]

    #region [- ConfigureCors -]

    /// <summary>
    /// Adds cross-origin resources and specified policies to service collection
    /// <para>- With specified headers and methods policies</para>
    /// <para>- Without specified origins</para>
    /// </summary>
    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(p => p.AddPolicy(CorsPolicies.Name, q =>
        {
            q.AllowAnyOrigin().WithHeaders(CorsPolicies.Headers).WithMethods(CorsPolicies.Methods);
        }));
    }
    #endregion

    #region [- ConfigureVersioningAndSwagger -]

    /// <summary>
    /// Adds api versioning and swagger using header versioning type to service collection
    /// </summary>
    private static void ConfigureVersioningAndSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        #region [- Versioning -]

        // Add api versioning using header versioning type
        services.AddApiVersioning(p =>
        {
            p.DefaultApiVersion = new ApiVersion(1, 0);
            p.ReportApiVersions = true;
            p.AssumeDefaultVersionWhenUnspecified = true;
            p.ApiVersionReader = new HeaderApiVersionReader("version");
        });
        #endregion

        #region [- Swagger -]

        // Add swagger settings with header versioning type
        services.AddSwaggerGen(p =>
        {
            APIDocument.APIVersions.ToList().ForEach(version =>
            {
                p.SwaggerDoc($"v{version}", new OpenApiInfo { Title = APIDocument.SwaggerTitle, Version = $"v{version}" });
            });
            p.EnableAnnotations();
            p.OperationFilter<VersioningFilter>();
            SwitchVersions(p);
        });

        #endregion
    }
    #endregion

    #region [- ConfigureRedis -]
    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetSection(RedisDBOptions.SectionName).Get<RedisDBOptions>();

        var dbConfig = new ConfigurationOptions
        {
            EndPoints = { { dbOptions.Server, dbOptions.Port } },
            Password = dbOptions.Password,
            AbortOnConnectFail = dbOptions.AbortOnConnectFail
        };

        services.AddSingleton<IConnectionMultiplexer>(p => ConnectionMultiplexer.Connect(dbConfig));
    }
    #endregion

    #region [- ConfigureIpRateLimiting -]
    private static void ConfigureIpRateLimiting(IServiceCollection services)
    {
        services.AddMemoryCache();

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
            options.GeneralRules = new List<RateLimitRule>
                    {
                        new RateLimitRule
                        {
                            Endpoint = "GET:/Item/*",                           
                            Period = "15s",
                            Limit = 5,
                        }
                    };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();
    }
    #endregion

    #region [- ConfigureAllServices -]

    /// <summary>
    /// Extension method that configures all needed services for app
    /// </summary>
    public static IServiceCollection ConfigureAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureCors(services);
        ConfigureIpRateLimiting(services);
        services.AddControllers().AddJsonOptions(p =>
        {
            p.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            p.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        ConfigureVersioningAndSwagger(services);

        ConfigureRedis(services,configuration);

        var connection = configuration.GetConnectionString("DB");
        if (!string.IsNullOrEmpty(connection))
            services.ConfigureDbContext(connection);

        services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IItemService,ItemService>();

        #region [- HealthChecks -]

        services.AddHealthChecks().AddCheck<RedisHealthChecking>("Redis")
                                  .AddCheck<SqlHealthChecking>("SQL");

        #endregion

        #region [- HttpLogging : Request & Response -]

        services.AddHttpLogging(p =>
        {
            p.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
        });
        #endregion

        return services;
    }
    #endregion

    #endregion
}
