using iLG.API.Constants;
using iLG.API.Extensions;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iLG.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        /// <summary>
        /// Login user
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.LOGGED_IN);
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("logout")]
        public async Task<ActionResult<ApiResponse>> Logout()
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
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var errorMessage = await _userService.Logout(userId, token);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.LOGGED_OUT);
        }

        /// <summary>
        /// Register user
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
        /// Account activation
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.ACC_ACTIVATED);
        }

        /// <summary>
        /// Resend OTP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("resend-otp")]
        public async Task<ActionResult<ApiResponse>> Resend([FromBody] ResendOtpRequest request)
        {
            var response = new ApiResponse();
            var isSent = await _emailService.SendActivationEmail(request.Email, true);

            if (!isSent)
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = Message.Error.Common.EMAIL_ERROR
                });

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.NEW_OTP);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("change-password")]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
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

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var errorMessage = await _userService.ChangePassword(request, email);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.PW_CHANGED);
        }

        /// <summary>
        /// Send a new password for user via email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("forgot-password")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = new ApiResponse();
            var forgotPassword = await _userService.ForgotPassword(request);

            if (!string.IsNullOrEmpty(forgotPassword.Item2))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = forgotPassword.Item2
                });

                if (forgotPassword.Item2 == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            await _emailService.SendNewPasswordEmail(request.Email, forgotPassword.Item1);

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.NEW_PASSWORD);
        }
    }
}