
namespace ToDoApp.API.Utils.HealthChecks;

public class HealthCheckingResponse
{
    public string GlobalStatus { get; set; } = null!;
    public IEnumerable<AppComponent>? Components { get; set; }
    public TimeSpan TotalDuration { get; set; }
}
