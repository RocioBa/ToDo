using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;
using ToDoApp.API.DTOs.Responses;
using ToDoApp.API.Utils.Exceptions;

namespace ToDoApp.API.Middlewares;

public class ErrorHandlingMiddleware
{
    #region [- Constructor -]

    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory logger)
    {
        _next = next;
        _logger = logger.CreateLogger("ToDoApp.API(ErrorHandlingMiddleware)");
    }
    #endregion

    #region [- Methods -]

    #region [- Invoke -]
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleError(context, ex);
        }
    }
    #endregion

    #region [- HandleError -]
    private Task HandleError(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var data = new ResponseDTO() { Data = null };

        switch (ex)
        {
            case AppException:
                {
                    data.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(message: ex.Message, exception: null);
                    break;
                }
            default:
                {
                    data.Message = "Internal server error has occurred";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogCritical(message: ex.GetType().Name, exception: ex);
                    break;
                }
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(data));
    }
    #endregion

    #endregion

}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}