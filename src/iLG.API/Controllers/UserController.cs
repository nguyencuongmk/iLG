﻿using iLG.API.Constants;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            var loginResponse = await _userService.Login(request);

            if (!string.IsNullOrEmpty(loginResponse.Item2))
            {
                if (loginResponse.Item2 == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError, loginResponse.Item2));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest, loginResponse.Item2);
                return BadRequest(result);
            }

            response.Data = loginResponse.Item1;

            return response.GetResult(StatusCodes.Status200OK);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterRequest request)
        {
            // Lấy thông tin người dùng từ biến môi trường ClaimsPrincipal
            var username = User.Identity.Name;

            return Ok($"Hello, {username}! This is a protected resource.");
        }
    }
}