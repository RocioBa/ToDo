using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ToDoApp.API.Utils.Swagger;
public static class SwaggerHelper
{
    #region [- Methods -]

    #region [- SwitchVersions -]
    /// <summary>
    /// Providing ability to switch between versions and display all api according versions in swagger
    /// </summary>
    public static void SwitchVersions(SwaggerGenOptions option)
    {
        option.DocInclusionPredicate((version, desc) =>
        {
            if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
            var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(q => q.Versions);
            var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(q => q.Versions);
            version = version.Replace("v", "");
            return versions.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version));
        });
    }
    #endregion

    #endregion
}
