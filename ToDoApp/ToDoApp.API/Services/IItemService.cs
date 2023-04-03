using ToDoApp.API.DTOs.Requests;
using ToDoApp.API.DTOs.Responses;
using ToDoApp.Data.Data.Models;

namespace ToDoApp.API.Services;

public interface IItemService
{
    Task<ResponseDTO> CreateItemAsync(ItemDTO data);
    Task<ResponseDTO> GetItemAsync(int id);
    Task<ResponseDTO> GetItemsAsync();
} 
