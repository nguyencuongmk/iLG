using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;

namespace iLG.API.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public TokenMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                var response = new ApiResponse();
                response.Errors.Add(new Error()
                {
                    ErrorMessage = "Invalid Token"
                });

                var result = response.GetResult(401);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(result);
            }
            else
            {
                var isValidToken = await _userService.VerifyToken(token);
                if (isValidToken)
                {
                    await _next(context);
                }
                else
                {
                    var response = new ApiResponse();
                    response.Errors.Add(new Error()
                    {
                        ErrorMessage = "Invalid Token"
                    });

                    var result = response.GetResult(401);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(result);
                }
            }
        }
    }
}