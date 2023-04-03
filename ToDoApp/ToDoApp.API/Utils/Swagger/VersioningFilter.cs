using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToDoApp.API.Utils.Swagger;
public class VersioningFilter : IOperationFilter
{
    /// <summary>
    /// Apply versioning filter using header versioning type
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var current_version = context.MethodInfo.GetCustomAttributes(true)
                                                .OfType<MapToApiVersionAttribute>()
                                                .SelectMany(p => p.Versions).ToList()
                                                .First().MajorVersion.ToString();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "version",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString(current_version)
            }
        });
    }
}
