using iLG.API.Models.Responses;
using iLG.Domain.Entities;
using System.Security.Claims;

namespace iLG.API.Services.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        string GetAccessTokenFromRequest(HttpRequest request);

        bool IsAccessTokenValid(string accessToken);

        Task<(TokenResponse, string)> GetNewToken(HttpRequest request);
    }
}