using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Domain.Entities;
using iLG.Domain.Enums;
using iLG.Infrastructure.Extentions;
using iLG.Infrastructure.Helpers;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace iLG.API.Services
{
    public class UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IRoleRepository roleRepository, IDistributedCache cache) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IDistributedCache _cache = cache;

        /// <summary>
        /// Activate user account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> Activation(ActivationRequest request)
        {
            #region Verify Request

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.Account.INVALID_EMAIL;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync(expression: u => u.Email == request.Email);

            if (user is null)
            {
                message = Message.Error.Common.SERVER_ERROR;
                return message;
            }

            //if (user.EmailConfirmed)
            //    return message;

            //var isValidOtp = OtpHelper.VerifyOTP(request.Otp, user.Otp, user.OtpExpiredTime);

            //if (!isValidOtp)
            //{
            //    message = Message.Error.User.INVALID_OTP;
            //    return message;
            //}

            //user.EmailConfirmed = true;
            await _userRepository.UpdateAsync(user);

            return message;

            #endregion Business Logic
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> ChangePassword(ChangePasswordRequest request, string email)
        {
            #region Verify Request

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(email))
            {
                message = Message.Error.Account.INVALID_EMAIL;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync(u => u.Email == email);

            if (user is null)
            {
                message = Message.Error.Common.SERVER_ERROR;
                return message;
            }

            var isValidPassword = PasswordHelper.VerifyPassword(request.OldPassword, user.PasswordHash);

            if (!isValidPassword)
            {
                message = "Invalid current password";
                return message;
            }

            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return message;

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

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return (string.Empty, message);
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.Account.INVALID_EMAIL;
                return (string.Empty, message);
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _userRepository.GetAsync(u => u.Email == request.Email);

            if (user is null)
            {
                message = Message.Error.Common.SERVER_ERROR;
                return (string.Empty, message);
            }

            string newPassword = PasswordHelper.GenerateRandomPassword(10);
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return (newPassword, message);

            #endregion Business Logic
        }

        /// <summary>
        /// Login user account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var response = new LoginResponse();

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return (response, message);
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.Account.INVALID_EMAIL;
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
                message = Message.Error.Account.LOGIN_FAILED;
                return (response, message);
            }

            if (user.IsLocked)
            {
                message = Message.Error.Account.ACCOUNT_LOCKED;
                return (response, message);
            }

            //if (!user.EmailConfirmed)
            //{
            //    message = Message.Error.User.EMAIL_NOT_YET_CONFIRMED;
            //    return (response, message);
            //}

            var userTokens = await _userTokenRepository.GetListAsync
            (
                expression: ut => ut.UserId == user.Id && ut.ExpiredTime > DateTime.UtcNow && !ut.IsDeleted,
                orderBy: o => o.OrderByDescending(ut => ut.ExpiredTime)
            );

            if (userTokens == null || !userTokens.Any())
            {
                var roles = _userRepository.GetRoles(user);

                if (roles.Count == 0)
                {
                    message = Message.Error.Common.SERVER_ERROR;
                    return (response, message);
                }

                var accessToken = JwtHelper.GenerateAccessToken(user.Email, user.Id, roles);

                if (string.IsNullOrEmpty(accessToken.Item1))
                {
                    message = Message.Error.Common.SERVER_ERROR;
                    return (response, message);
                }

                // Insert Access Token to Database
                user.UserTokens.Add(new UserToken
                {
                    Token = accessToken.Item1,
                    ExpiredTime = accessToken.Item2,
                    Platform = PlatformHelper.GetPlatformName(Environment.OSVersion.Platform),
                    MachineName = Environment.MachineName
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
                    message = Message.Error.Common.SERVER_ERROR;
                    return (response, message);
                }

                response.Email = user.Email;
                response.AccessToken = userToken.Token;
            }

            return (response, message);

            #endregion Business Logic
        }

        /// <summary>
        /// Logout user account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> Logout(int userId, string token)
        {
            #region Verify Request

            var message = string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var userToken = await _userTokenRepository.GetAsync(ut => ut.Token == token && ut.UserId == userId);

            if (userToken is null)
            {
                message = Message.Error.Common.SERVER_ERROR;
                return message;
            }

            userToken.ExpiredTime = DateTime.MinValue;
            await _userTokenRepository.UpdateAsync(userToken);

            return message;

            #endregion Business Logic
        }

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> Register(RegisterRequest request)
        {
            #region Verify Request

            if (request == null || string.IsNullOrEmpty(request?.Email) || string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.Otp))
                return Message.Error.Account.NOT_ENOUGH_INFO;

            if (!EmailHelper.IsValidEmail(request.Email))
                return Message.Error.Account.INVALID_EMAIL;

            if (await _userRepository.IsExistAsync(expression: u => u.Email == request.Email))
                return Message.Error.Account.EXISTS_EMAIL;

            var cacheOtp = await _cache.GetStringAsync(request.Email);

            if (!PasswordHelper.IsValidPassword(request.Password))
                return Message.Error.Account.INVALID_PASSWORD;

            if (string.IsNullOrEmpty(cacheOtp))
                return Message.Error.Account.EXPIRED_OTP;

            if (request.Otp != cacheOtp)
                return Message.Error.Account.INVALID_OTP;

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

        /// <summary>
        /// Verify user token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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