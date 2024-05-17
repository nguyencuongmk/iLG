using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserInfoController : ControllerBase
    {

        [HttpGet("search-suitable")]
        public async Task<ActionResult<ApiResponse>> SearchSuitableUser([FromBody] SearchSuitableRequest request)
        {
            var response = new ApiResponse();
            throw new NotImplementedException();
        }
    }
}