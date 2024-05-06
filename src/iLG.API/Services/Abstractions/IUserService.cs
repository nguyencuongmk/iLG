﻿using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserService
    {
        Task<(LoginResponse, string)> Login(LoginRequest request);

        Task<bool> VerifyToken(string token);
    }
}