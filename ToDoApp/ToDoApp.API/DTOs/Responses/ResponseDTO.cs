using System.Net;
using System.Text.Json.Serialization;

namespace ToDoApp.API.DTOs.Responses;

public class ResponseDTO
{
    public object? Data { get; set; }
    public string? Message { get; set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
}
