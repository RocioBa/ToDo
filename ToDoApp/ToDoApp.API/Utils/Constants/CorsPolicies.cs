namespace ToDoApp.API.Utils.Constants;

/// <summary>
/// Class that represents specified Policies For Cors Like HttpHeaders And HttpMethods
/// </summary>
public class CorsPolicies
{
    public const string Name = "ToDoApp";

    public static readonly string[] Headers = { "Authorization", "Version", "Accept-Language" };

    public static readonly string[] Methods = { "POST", "GET", "PUT", "DELETE" };
}
