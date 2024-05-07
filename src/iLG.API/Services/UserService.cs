using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Domain.Entities;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;

        public UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var response = new LoginResponse();

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                message = Message.Error.User.LOGIN_FAILED;
                return (response, message);
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync
            (
                expression: u => u.Email == request.Email && !u.IsDeleted
            );

            var isValidPassword = _userRepository.CheckPassword(user, request.Password);

            if (user == null || !isValidPassword)
            {
                message = Message.Error.User.LOGIN_FAILED;
                return (response, message);
            }

            if (user.IsLocked)
            {
                message = Message.Error.User.ACCOUNT_LOCKED;
                return (response, message);
            }

            if (!user.EmailConfirmed)
            {
                message = Message.Error.User.EMAIL_NOT_YET_CONFIRMED;
                return (response, message);
            }

            var userTokens = await _userTokenRepository.GetListAsync
            (
                expression: ut => ut.UserId == user.Id && ut.ExpiredTime > DateTime.Now && !ut.IsDeleted,
                orderBy: o => o.OrderByDescending(ut => ut.ExpiredTime)
            );

            if (userTokens == null || !userTokens.Any())
            {
                var roles = _userRepository.GetRoles(user);

                if (roles.Count == 0)
                {
                    message = Message.Error.SERVER_ERROR;
                    return (response, message);
                }

                var accessToken = JwtHelper.GenerateAccessToken(user.Email, user.Id, roles);

                if (string.IsNullOrEmpty(accessToken.Item1))
                {
                    message = Message.Error.SERVER_ERROR;
                    return (response, message);
                }

                // Insert Access Token to Database
                user.UserTokens.Add(new UserToken
                {
                    Token = accessToken.Item1,
                    ExpiredTime = accessToken.Item2, 
                    Platform = PlatformHelper.GetPlatformName(Environment.OSVersion.Platform),
                    MachineName = Environment.MachineName,
                    CreatedBy = "system"
                });

                await _userRepository.UpdateAsync(user);

                response.Email = user.Email;
                response.AccessToken = accessToken.Item1;
            }
            else
            {
                var userToken = userTokens.FirstOrDefault();

                if (userToken == null)
                {
                    message = Message.Error.SERVER_ERROR;
                    return (response, message);
                }

                response.Email = user.Email;
                response.AccessToken = userToken.Token;
            }

            return (response, message);

            #endregion Business Logic
        }

        public async Task<bool> VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var email = JwtHelper.GetEmailFromToken(token);

            if (string.IsNullOrEmpty(email))
                return false;

            var user = await _userRepository.GetAsync(expression: u => u.Email == email);

            if (user is null)
                return false;

            var isValidToken = await _userTokenRepository.IsExistAsync
            (
                expression: ut => ut.Token == token && ut.UserId == user.Id && ut.ExpiredTime > DateTime.UtcNow && !ut.IsDeleted
            );

            return isValidToken;
        }
    }
}