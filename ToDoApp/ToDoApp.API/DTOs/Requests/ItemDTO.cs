
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using ToDoApp.Data.Utils.Enums;

namespace ToDoApp.API.DTOs.Requests;

public class ItemDTO
{
    [SwaggerSchema("Title for to do item")]
    public string? Title { get; set; } 

    [SwaggerSchema("More explain about to do item")]
    public string? Description { get; set; }
    public Priority? Priority { get; set; }
}
