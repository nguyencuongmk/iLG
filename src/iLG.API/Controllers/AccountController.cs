using iLG.API.Constants;
using iLG.API.Extensions;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace iLG.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController(IUserService userService, IEmailService emailService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IEmailService _emailService = emailService;

        /// <summary>
        /// Sign in an account
        /// </summary-=  >
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<ActionResult<ApiResponse>> Signin([FromBody] SigninRequest request)
        {
            var response = new ApiResponse();
            var loginResponse = await _userService.SignIn(request);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.SIGNED_IN);
        }

        /// <summary>
        /// Sign out an account
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sigout")]
        public async Task<ActionResult<ApiResponse>> Signout()
        {
            var response = new ApiResponse();
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = Message.Error.Account.NOT_AUTH
                });

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            _ = int.TryParse(user.FindFirst("userId")?.Value, out int userId);
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var errorMessage = await _userService.SignOut(userId, token);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.SIGNED_OUT);
        }

        /// <summary>
        /// Sign up a new account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse>> SignUp([FromBody] SignupRequest request)
        {
            var response = new ApiResponse();
            var errorMessage = await _userService.SignUp(request);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.SIGNED_UP);
        }

        [HttpPost("otp")]
        public async Task<ActionResult<ApiResponse>> SendOtpEmail([FromBody] SendOtpRequest request)
        {
            var response = new ApiResponse();
            var errorMessage = await _emailService.SendOtpEmail(request);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = errorMessage
                });

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.OTP);
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
                    ErrorMessage = Message.Error.Account.NOT_AUTH
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.PW_CHANGED);
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.Account.NEW_PASSWORD);
        }
    }
}