using iLG.Domain.Entities;

namespace iLG.API.Services.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        Task<bool> IsRefreshTokenValid(string refreshToken);

        int? GetUserIdFromAccessToken(string accessToken);

        Task<string> GetNewAccessToken(string refreshToken);
    }
}
