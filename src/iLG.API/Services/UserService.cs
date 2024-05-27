using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Domain.Entities;
using iLG.Infrastructure.Helpers;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace iLG.API.Services
{
    public class UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IRoleRepository roleRepository, IDistributedCache cache, ITokenService tokenService) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IDistributedCache _cache = cache;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> ChangePassword(ChangePasswordRequest request, string? email)
        {
            #region Verify Request

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(request?.OldPassword) || string.IsNullOrEmpty(request?.NewPassword))
                return Message.Error.User.NOT_ENOUGH_INFO;

            if (!EmailHelper.IsValidEmail(email))
                return Message.Error.User.INVALID_EMAIL;

            if (!PasswordHelper.IsValidPassword(request.NewPassword))
                return Message.Error.User.INVALID_PASSWORD;

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (user is null)
                return Message.Error.Common.SERVER_ERROR;

            if (user.UserInfo is null)
                return Message.Error.UserInfo.NOT_YET_UPDATED;

            var isValidPassword = PasswordHelper.VerifyPassword(request.OldPassword, user.PasswordHash);

            if (!isValidPassword)
                return Message.Error.User.INVALID_CURRENT_PASSWORD;

            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return string.Empty;

            #endregion Business Logic
        }

        /// <summary>
        /// Generate new password for user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(string, string)> ForgotPassword(ForgotPasswordRequest request)
        {
            #region Verify Request

            if (request == null || string.IsNullOrEmpty(request.Email))
                return (string.Empty, Message.Error.User.NOT_ENOUGH_INFO);

            if (!EmailHelper.IsValidEmail(request.Email))
                return (string.Empty, Message.Error.User.INVALID_EMAIL);

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted);

            if (user is null)
                return (string.Empty, Message.Error.Common.SERVER_ERROR);

            string newPassword = PasswordHelper.GenerateRandomPassword(10);
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return (newPassword, string.Empty);

            #endregion Business Logic
        }

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(SigninResponse, string)> SignIn(SigninRequest request)
        {
            #region Verify Request

            var response = new SigninResponse();

            if (string.IsNullOrEmpty(request?.Email) || string.IsNullOrEmpty(request?.Password))
                return (response, Message.Error.User.NOT_ENOUGH_INFO);

            if (!EmailHelper.IsValidEmail(request.Email))
                return (response, Message.Error.User.INVALID_EMAIL);

            if (!PasswordHelper.IsValidPassword(request.Password))
                return (response, Message.Error.User.INVALID_PASSWORD);

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync
            (
                expression: u => u.Email == request.Email && !u.IsDeleted
            );

            var isValidPassword = _userRepository.CheckPassword(user, request.Password);

            if (user == null || !isValidPassword)
                return (response, Message.Error.User.LOGIN_FAILED);

            if (user.IsLocked)
                return (response, Message.Error.User.USER_LOCKED);

            var machineName = Environment.MachineName;
            var platform = PlatformHelper.GetPlatformName(Environment.OSVersion.Platform);
            var userToken = await _userTokenRepository.GetAsync(expression: ut => ut.UserId == user.Id && ut.MachineName == machineName && ut.Platform == platform);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var expiredTime = DateTime.UtcNow.AddDays(7);

            if (userToken is null)
            {
                userToken = new UserToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiredTime = expiredTime,
                    Platform = platform,
                    MachineName = machineName
                };
                await _userTokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.Token = refreshToken;
                userToken.ExpiredTime = expiredTime;
                await _userTokenRepository.UpdateAsync(userToken);
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            if (string.IsNullOrEmpty(accessToken))
                return (response, Message.Error.Common.SERVER_ERROR);

            response.Email = user.Email;
            response.AccessToken = accessToken;
            response.RefreshToken = userToken.Token;
            response.IsUpdatedInfo = user.UserInfo is not null;

            return (response, string.Empty);

            #endregion Business Logic
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SignOut(int userId, string accessToken)
        {
            #region Business Logic

            var machineName = Environment.MachineName;
            var platform = PlatformHelper.GetPlatformName(Environment.OSVersion.Platform);
            var userToken = await _userTokenRepository.GetAsync(ut => ut.UserId == userId && ut.MachineName == machineName && ut.Platform == platform);

            if (userToken is null)
                return Message.Error.Common.SERVER_ERROR;

            userToken.ExpiredTime = DateTime.MinValue;
            await _userTokenRepository.UpdateAsync(userToken);
            _cache.Remove(accessToken);

            return string.Empty;

            #endregion Business Logic
        }

        /// <summary>
        /// Sign up
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> SignUp(SignupRequest request)
        {
            #region Verify Request

            if (request == null || string.IsNullOrEmpty(request?.Email) || string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.Otp))
                return Message.Error.User.NOT_ENOUGH_INFO;

            if (!EmailHelper.IsValidEmail(request.Email))
                return Message.Error.User.INVALID_EMAIL;

            if (await _userRepository.IsExistAsync(expression: u => u.Email == request.Email && !u.IsDeleted))
                return Message.Error.User.EXISTS_EMAIL;

            var cacheOtp = await _cache.GetStringAsync(request.Email);

            if (!PasswordHelper.IsValidPassword(request.Password))
                return Message.Error.User.INVALID_PASSWORD;

            if (string.IsNullOrEmpty(cacheOtp))
                return Message.Error.User.EXPIRED_OTP;

            if (request.Otp != cacheOtp)
                return Message.Error.User.INVALID_OTP;

            #endregion Verify Request

            #region Business Logic

            var role = await _roleRepository.GetAsync
            (
                 expression: r => r.Code == "USR"
            );

            if (role is null)
                return Message.Error.Common.SERVER_ERROR;

            var user = new User
            {
                Email = request.Email,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                Roles =
                [
                    role
                ]
            };

            await _userRepository.CreateAsync(user);

            return string.Empty;

            #endregion Business Logic
        }
    }
}