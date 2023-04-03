namespace ToDoApp.API.Utils.Exceptions;

/// <summary>
/// Using to logging exceptions with error level   
/// </summary>
public class AppException : Exception
{
    public AppException(string? message) : base(message) { }
}

