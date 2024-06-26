﻿using iLG.API.Constants;
using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace iLG.API.Middleware
{
    public class TokenMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task Invoke(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>() is null)
            {
                await _next(context);
            }
            else
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var tokenService = scopedServices.GetRequiredService<ITokenService>();

                    var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

                    if (string.IsNullOrEmpty(token))
                    {
                        var response = new ApiResponse();
                        response.Errors.Add(new Error()
                        {
                            ErrorMessage = Message.Error.User.INVALID_AC_TOKEN
                        });

                        var result = response.GetResult(StatusCodes.Status401Unauthorized);
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(result);
                    }
                    else
                    {
                        var isValidToken = tokenService.IsAccessTokenValid(token);
                        if (isValidToken)
                        {
                            // Todo: Check Permission
                            await _next(context);
                        }
                        else
                        {
                            var response = new ApiResponse();
                            response.Errors.Add(new Error()
                            {
                                ErrorMessage = Message.Error.User.INVALID_AC_TOKEN
                            });

                            var result = response.GetResult(StatusCodes.Status401Unauthorized);
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(result);
                        }
                    }
                }
            }
        }
    }
}