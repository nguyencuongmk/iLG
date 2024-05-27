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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IEmailService emailService, ITokenService tokenService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IEmailService _emailService = emailService;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Sign in
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.SIGNED_IN);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sigout")]
        public async Task<ActionResult<ApiResponse>> Signout()
        {
            var response = new ApiResponse();
            _ = int.TryParse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId);
            var accessToken = _tokenService.GetAccessTokenFromRequest(Request);
            var errorMessage = await _userService.SignOut(userId, accessToken);

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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.SIGNED_OUT);
        }

        /// <summary>
        /// Sign up
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.SIGNED_UP);
        }

        /// <summary>
        /// Send OTP code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

            return response.GetResult(StatusCodes.Status200OK, Message.Success.User.OTP);
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

        /// <summary>
        /// Get new Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse>> RefreshToken()
        {
            var response = new ApiResponse();
            var tokenResponse = await _tokenService.GetNewToken(Request);

            if (!string.IsNullOrEmpty(tokenResponse.Item2))
            {
                response.Errors.Add(new Error
                {
                    ErrorMessage = tokenResponse.Item2
                });

                if (tokenResponse.Item2 == Message.Error.Common.SERVER_ERROR)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.GetResult(StatusCodes.Status500InternalServerError));
                }

                var result = response.GetResult(StatusCodes.Status400BadRequest);
                return BadRequest(result);
            }

            response.Data = tokenResponse.Item1;

            return response.GetResult(StatusCodes.Status200OK);
        }
    }
}