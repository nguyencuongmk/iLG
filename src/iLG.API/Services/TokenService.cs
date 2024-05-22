using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Domain.Entities;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iLG.API.Services
{
    public class TokenService(IOptions<AppSettings> options, IUserTokenRepository userTokenRepository, IUserRepository userRepository) : ITokenService
    {
        private readonly AppSettings _appSettings = options.Value;
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository;
        private readonly IUserRepository _userRepository = userRepository;

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
                var expires = DateTime.UtcNow.AddSeconds(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _appSettings.JwtSettings.Audience,
                    Issuer = _appSettings.JwtSettings.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
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

        public async Task<bool> IsRefreshTokenValid(string refreshToken)
        {
            var token = await _userTokenRepository.GetAsync(ut => ut.Token == refreshToken && ut.ExpiredTime > DateTime.Now);
            return token != null;
        }

        public async Task<string> GetNewAccessToken(string refreshToken)
        {
            // Lấy Access Token mới từ Refresh Token
            var token = await _userTokenRepository.GetAsync(t => t.Token == refreshToken && t.ExpiredTime > DateTime.Now);
            if (token == null)
                return null;

            var user = await _userRepository.GetAsync(u => u.Id == token.UserId);
            if (user == null)
                return null;

            var newAccessToken = GenerateAccessToken(user);
            return newAccessToken;
        }

        public int? GetUserIdFromAccessToken(string accessToken)
        {
            // Lấy ID người dùng từ Access Token
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);
                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}