using iLG.API.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iLG.API.Helpers
{
    public static class JwtHelper
    {
        private static AppSettings _appSettings;

        public static void Initialize(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public static (string, DateTime) GenerateAccessToken(string email, int userId, List<string?> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret);
                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Email, email),
                    new(JwtRegisteredClaimNames.Sub, userId.ToString())
                };

                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var expires = DateTime.UtcNow.AddHours(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _appSettings.JwtSettings.Audience,
                    Issuer = _appSettings.JwtSettings.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return (tokenHandler.WriteToken(token), expires);
            }
            catch
            {
                return (string.Empty, new DateTime());
            }
        }

        public static string? GetEmailFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            var emailClaim = claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email);

            if (emailClaim is not null)
            {
                var email = emailClaim.Value;
                return email;
            }

            return null;
        }
    }
}