

namespace ToDoApp.API.Utils.HealthChecks;

public class AppComponent
{
    public string Status { get; set; } = null!;
    public string Component { get; set; } = null!;
    public string? ErrorDescription { get; set; }
}
