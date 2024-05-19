using iLG.API.Constants;
using iLG.API.Extensions;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserInfoController(IUserInfoService userInfoService) : ControllerBase
    {
        private readonly IUserInfoService _userInfoService = userInfoService;

        [HttpGet("search-suitable")]
        public async Task<ActionResult<ApiResponse>> SearchSuitableUser(int minAge, int maxAge, string gender, int pageIndex = 1, int pageSize = 5)
        {
            var response = new ApiResponse();
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = Message.Error.User.NOT_AUTH
                });

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            _ = int.TryParse(user.FindFirst("userId")?.Value, out int userId);

            var userInfos = await _userInfoService.SearchSuitableUser(userId, minAge, maxAge, gender, pageIndex, pageSize);

            if (!string.IsNullOrEmpty(userInfos.Item2))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = userInfos.Item2
                });

                if (userInfos.Item2 == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            response.Data = userInfos.Item1;

            return response.GetResult(StatusCodes.Status200OK);
        }
    }
}