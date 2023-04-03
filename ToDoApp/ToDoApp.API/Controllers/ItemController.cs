using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoApp.API.DTOs.Requests;
using ToDoApp.API.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ToDoApp.API.Controllers
{
    [Route("Item")]
    [ApiController]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class ItemController : BaseController
    {
        #region [- Constructor -]

        private readonly IItemService _service;
        public ItemController(IItemService service)
        {
            _service = service;
        }
        #endregion

        #region [- Actions -]

        #region [- CreateAsync -]

        [HttpPost("CreateAsync")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Create one item in to do list")]
        public async Task<IActionResult> CreateAsync([FromBody] ItemDTO data)
        {
            var res =await _service.CreateItemAsync(data);
            return APIResponse(res);
        }
        #endregion

        #region [- GetAsync -]

        [HttpGet("GetAsync/{id}")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Get item with id from to do list")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var res = await _service.GetItemAsync(id);
            return APIResponse(res);
        }
        #endregion

        #region [- GetAllAsync -]

        [HttpGet("GetAllAsync")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Get all items from to do list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _service.GetItemsAsync();
            return APIResponse(res);
        }
        #endregion

        #endregion
    }
}
