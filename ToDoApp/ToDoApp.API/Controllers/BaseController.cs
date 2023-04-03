

using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.DTOs.Responses;

namespace ToDoApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region [- Methods -]

        #region [- APIResponse -]

        [NonAction]
        public ObjectResult APIResponse(ResponseDTO response)
        {
            return new ObjectResult(new { data = response.Data, message = response.Message })
            {
                StatusCode = (int)response.StatusCode
            };
        }
        #endregion 

        #endregion 
    } 
}
