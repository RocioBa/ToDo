using Serilog;


namespace ToDoApp.API.Infrastructure;
public static class ConfigureAppBuilder
{
    #region [- Methods -]

    #region [- ConfigureSerilog -]
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        // Now we have http logging with console
        // If we want http logging with file, we can just add Information level log in section serilog in appsettings.json

        var config = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                                              .WriteTo.Console(outputTemplate: "{NewLine} {NewLine} {Timestamp:HH:mm:ss} | {Level} | Category({SourceContext}) | EventId({EventId}) {NewLine} ===> Message({Message}) {NewLine} {Exception}")
                                              .Enrich.FromLogContext()
                                              .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(config);

        return builder;
    }
    #endregion

    #endregion
}
