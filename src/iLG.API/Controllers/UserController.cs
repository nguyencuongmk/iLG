using iLG.API.Constants;
using iLG.API.Extensions;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace iLG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
                response.Errors.Add(new Error
                {
                    ErrorMessage = loginResponse.Item2
                });

                if (loginResponse.Item2 == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            response.Data = loginResponse.Item1;

            return response.GetResult(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = new ApiResponse();
            var errorMessage = await _userService.Register(request);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = errorMessage
                });

                if (errorMessage == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            await _emailService.SendActivationEmail(request.Email);

            return response.GetResult(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Account Activation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("activation")]
        public async Task<ActionResult<ApiResponse>> Activation([FromBody] ActivationRequest request)
        {
            var response = new ApiResponse();
            var errorMessage = await _userService.Activation(request);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = errorMessage
                });

                if (errorMessage == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            return response.GetResult(StatusCodes.Status200OK);
        }
    }
}