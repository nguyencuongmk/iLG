using iLG.API.Extensions;
using iLG.API.Helpers;
using iLG.API.Models.Responses;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequest request)
        {
            var response = new ApiResponse();
            var currentUser = HttpContext.User;

            if (currentUser != null && currentUser.Identity.IsAuthenticated)
            {
                response = response.GetResult(400);
                return BadRequest(response);
            }

            var user = await _userRepository.GetAsync
            (
                expression: x => x.Email == request.Email && !x.IsLocked && x.EmailConfirmed
            );

            var isValidPassword = _userRepository.CheckPassword(user, request.Password);

            var roles = _userRepository.GetRoles(user);
            var accessToken = JwtHelper.GenerateAccessToken(user.Email, user.Id, roles);
            response = new ApiResponse(accessToken.Item1);

            return response.GetResult(200);
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            // Lấy thông tin người dùng từ biến môi trường ClaimsPrincipal
            var username = User.Identity.Name;

            return Ok($"Hello, {username}! This is a protected resource.");
        }
    }
}