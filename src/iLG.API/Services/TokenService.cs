using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Domain.Entities;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iLG.API.Services
{
    public class TokenService(IOptions<AppSettings> options, IUserTokenRepository userTokenRepository, IDistributedCache cache) : ITokenService
    {
        private readonly AppSettings _appSettings = options.Value;
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository;
        private readonly IDistributedCache _cache = cache;

        public string GenerateAccessToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret);
                var claimList = new List<Claim>
                {
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                claimList.AddRange(user.Roles.Select(r => r.Name).Select(role => new Claim(ClaimTypes.Role, role)));
                //var expires = DateTime.UtcNow.AddHours(1);
                var expires = DateTime.UtcNow.AddMinutes(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _appSettings.JwtSettings.Audience,
                    Issuer = _appSettings.JwtSettings.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var sToken = tokenHandler.CreateToken(tokenDescriptor);
                var token =  tokenHandler.WriteToken(sToken);

                _cache.SetString($"{token}", user.Id.ToString(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(expires)
                });

                return token;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<(TokenResponse, string)> GetNewToken(HttpRequest request)
        {
            var response = new TokenResponse();
            var accessToken = GetAccessTokenFromRequest(request);
            var refreshToken = GetRefreshTokenFromRequest(request);

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return (response, Message.Error.Account.EMPTY_TOKEN);

            if (IsAccessTokenValid(accessToken))
            {
                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken;
                return (response, string.Empty);
            }

            var principal = GetPrincipalFromExpiredToken(accessToken);
            _ = int.TryParse(principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId);
            var machineName = Environment.MachineName;
            var platform = PlatformHelper.GetPlatformName(Environment.OSVersion.Platform);
            var userToken = await _userTokenRepository.GetAsync(ut => ut.UserId == userId && ut.Token == refreshToken && ut.ExpiredTime > DateTime.Now && ut.MachineName == machineName && ut.Platform == platform);

            if (userToken is null)
                return (response, Message.Error.Account.INVALID_RF_TOKEN);

            var newAccessToken = GenerateAccessToken(userToken.User);
            var newRefreshToken = GenerateRefreshToken();

            userToken.Token = newRefreshToken;
            userToken.ExpiredTime = DateTime.UtcNow.AddDays(7);
            await _userTokenRepository.UpdateAsync(userToken);

            response.AccessToken = newAccessToken;
            response.RefreshToken = newRefreshToken;

            return (response, string.Empty);
        }

        public bool IsAccessTokenValid(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = _appSettings.JwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidateAudience = true,
                    ValidAudience = _appSettings.JwtSettings.Audience,
                };

                var principal = new JwtSecurityTokenHandler().ValidateToken(accessToken, validationParameters, out _);

                var userId = _cache.GetString($"{accessToken}");
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetAccessTokenFromRequest(HttpRequest request)
        {
            var authorizationHeader = request.Headers.Authorization.ToString();

            if (!string.IsNullOrEmpty(authorizationHeader))
                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    return authorizationHeader["Bearer ".Length..].Trim();

            return string.Empty;
        }

        private string GetRefreshTokenFromRequest(HttpRequest request) => request.Headers["x-Refresh-Token"].ToString();

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _appSettings.JwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret)),
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidAudience = _appSettings.JwtSettings.Audience,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}