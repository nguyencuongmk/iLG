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

namespace iLG.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _roleRepository = roleRepository;
        }

        public async Task<string> Activation(ActivationRequest request)
        {
            #region Verify Request

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.User.INVALID_EMAIL;
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

            var isValidOtp = OTPHelper.VerifyOTP(request.Otp, user.Otp, user.OtpExpiredTime);

            if (!isValidOtp)
            {
                message = Message.Error.User.INVALID_OTP;
                return message;
            }

            user.EmailConfirmed = true;
            user.OtpExpiredTime = DateTime.MinValue;
            await _userRepository.UpdateAsync(user);

            return message;

            #endregion Business Logic
        }

        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var response = new LoginResponse();

            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return (response, message);
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.User.INVALID_EMAIL;
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

        public async Task<string> Register(RegisterRequest request)
        {
            #region Verify Request

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(request?.Email) || string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.FullName) || string.IsNullOrEmpty(request?.Gender))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.Error.User.INVALID_EMAIL;
                return message;
            }

            if (await _userRepository.IsExistAsync(expression: u => u.Email == request.Email))
            {
                message = Message.Error.User.EXISTS_EMAIL;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var role = await _roleRepository.GetAsync
            (
                 expression: r => r.Code == "USR"
            );

            if (role is null)
            {
                message = Message.Error.Common.SERVER_ERROR;
                return message;
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                UserInfo = new UserInfo
                {
                    FullName = request.FullName,
                    Age = request.Age,
                    Gender = request.Gender.ToEnum<Gender>()
                },
                Roles =
                [
                    role
                ],
                Otp = OTPHelper.GenerateOTP(),
                OtpExpiredTime = DateTime.UtcNow.AddMinutes(2),
            };

            await _userRepository.CreateAsync(user);

            return message;

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