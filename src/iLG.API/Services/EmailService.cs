using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Models.Requests;
using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace iLG.API.Services
{
    public class EmailService(IOptions<AppSettings> options, IUserRepository userRepository, IDistributedCache cache) : IEmailService
    {
        private readonly AppSettings _appSettings = options.Value;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IDistributedCache _cache = cache;

        public async Task<string> SendOtpEmail(SendOtpRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
                return Message.Error.Account.NOT_ENOUGH_INFO;

            if (!EmailHelper.IsValidEmail(request.Email))
                return Message.Error.Account.INVALID_EMAIL;

            if (await _userRepository.IsExistAsync(expression: u => u.Email == request.Email && !u.IsDeleted))
                return Message.Error.Account.EXISTS_EMAIL;

            var cacheOtp = await _cache.GetStringAsync(request.Email);

            if (!string.IsNullOrEmpty(cacheOtp))
                return Message.Error.Account.CACHE_OTP;

            var otp = OtpHelper.GenerateOTP();

            await _cache.SetStringAsync(request.Email, otp, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(2))
            });

            var subject = "Sign up OTP code";
            var body = $@"<p>The OTP code to sign up is: <b>{otp}</b></p>
                                <p>Best Regard,</p>
                                <p>iLG Support</p>";

            await Send(_appSettings.MailSettings.MailServerUsername, _appSettings.MailSettings.MailServerPassword, [request.Email], [], subject, body);
            return string.Empty;
        }

        public async Task<bool> SendNewPasswordEmail(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;

            if (!EmailHelper.IsValidEmail(email))
                return false;

            if (!await _userRepository.IsExistAsync(expression: u => u.Email == email))
                return false;

            var user = await _userRepository.GetAsync(expression: u => u.Email == email);

            if (user is null)
                return false;

            var subject = "Your new password";
            var body = $@"<p>Hi {user?.UserInfo.FullName},</p>
                                <p>Your new password is: <b>{password}</b></p>
                                <p>Best Regard,</p>
                                <p>iLG Support</p>";

            await Send(_appSettings.MailSettings.MailServerUsername, _appSettings.MailSettings.MailServerPassword, [email], [], subject, body);
            return true;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="senderEmail"></param>
        /// <param name="senderPassword"></param>
        /// <param name="toEmails"></param>
        /// <param name="ccEmails"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private async Task Send(string senderEmail, string senderPassword, string[] toEmails, string[] ccEmails, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                Host = _appSettings.MailSettings.MailServer,
                Port = _appSettings.MailSettings.MailServerPort,
                EnableSsl = _appSettings.MailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            using var mail = new MailMessage()
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.From = new MailAddress(senderEmail, "iLG");

            foreach (string toEmail in toEmails)
            {
                mail.To.Add(toEmail);
            }

            foreach (string ccEmail in ccEmails)
            {
                mail.CC.Add(ccEmail);
            }

            await smtpClient.SendMailAsync(mail);
        }
    }
}