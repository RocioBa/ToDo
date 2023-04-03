using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using ToDoApp.API.DTOs.Requests;
using ToDoApp.API.DTOs.Responses;
using ToDoApp.Data.Data.Context;
using ToDoApp.Data.Data.Models;
using ToDoApp.Data.Repository;

namespace ToDoApp.API.Services;

public class ItemService : IItemService
{
    #region [- Constructor -]

    private readonly IBaseRepository<Item,ToDoAppContext> _repo;
    private readonly IMapper _mapper;
    private readonly IDatabase _redisDb;
    public ItemService(IBaseRepository<Item,ToDoAppContext> repo, IMapper mapper, IConnectionMultiplexer redis)
    {
        _repo = repo;
        _mapper = mapper;
        _redisDb = redis.GetDatabase();
    }

    private const string Items = "ToDoItems";
    private const string Item = "ToDoItem";

    #endregion

    #region [- Methods -]

    #region [- CreateItemAsync -]
    public async Task<ResponseDTO> CreateItemAsync(ItemDTO data)
    {
        var item = _mapper.Map<Item>(data);
        _repo.Add(item);
        var result = await _repo.SaveChangesAsync(false);

        if (!result)
            return ManageResponse(HttpStatusCode.InternalServerError, null, "Internal server error has occurred");

        var items = await _repo.GetAllAsync();
        await _redisDb.StringSetAsync(Items, JsonSerializer.Serialize(items), TimeSpan.FromMinutes(15));

        return ManageResponse(HttpStatusCode.OK, null, null);
    }
    #endregion

    #region [- GetItemAsync -]
    public async Task<ResponseDTO> GetItemAsync(int id)
    {
        if (await _redisDb.KeyExistsAsync(Item))
        {
            var result = await _redisDb.StringGetAsync(Item);
            return ManageResponse(HttpStatusCode.OK, new { items = JsonSerializer.Deserialize<Item>(result) }, null);
        }

        if (!await _repo.DbSet.AnyAsync(p => p.Id == id))
            return ManageResponse(HttpStatusCode.NotFound,null, "Your requested item not found");

        var data = await _repo.DbSet.AsNoTracking().FirstAsync(p => p.Id == id);
        await _redisDb.StringSetAsync(Item, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(15));
        return ManageResponse(HttpStatusCode.OK, new { item = data }, null);
    }
    #endregion

    #region [- GetItemsAsync -]
    public async Task<ResponseDTO> GetItemsAsync()
    {
        if(await _redisDb.KeyExistsAsync(Items))
        {
            var result = await _redisDb.StringGetAsync(Items);
            return ManageResponse(HttpStatusCode.OK, new { items = JsonSerializer.Deserialize<List<Item>>(result) }, null);
        }

        var data = await _repo.GetAllAsync();
        await _redisDb.StringSetAsync(Items, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(15));    
        return ManageResponse(HttpStatusCode.OK, new { items = data }, null);
    }
    #endregion

    #region [- ManageResponse -]

    /// <summary>
    /// Creates response with specified structure
    /// </summary>
    private ResponseDTO ManageResponse(HttpStatusCode statusCode, object? data, string? message)
    {
        return new ResponseDTO()
        {
            StatusCode = statusCode,
            Data = data,
            Message = message
        };
    }
    #endregion

    #endregion
     
}
