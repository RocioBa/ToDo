
using System.ComponentModel.DataAnnotations;
using ToDoApp.Data.Utils.Enums;

namespace ToDoApp.Data.Data.Models;
public class Item 
{
    public int Id { get; set; }

    [RegularExpression(@"^[a-zA-Z\s]+$")]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime RegisterDate { get; set; }
    public Priority Priority { get; set; } 
}
   