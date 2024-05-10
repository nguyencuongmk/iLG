using Azure.Core;
using iLG.API.Constants;
using iLG.API.Helpers;
using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace iLG.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public EmailService(IOptions<AppSettings> options, IUserRepository userRepository)
        {
            _appSettings = options.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> SendActivationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (!EmailHelper.IsValidEmail(email))
                return false;

            if (await _userRepository.IsExistAsync(expression: u => u.Email == email))
                return false;

            var otp = (await _userRepository.GetAsync(expression: u => u.Email == email))?.Otp;

            if (string.IsNullOrEmpty(otp))
                return false;

            var fromAddress = new MailAddress(_appSettings.MailSettings.MailServerUsername, "iLG");
            var toAddress = new MailAddress(email);
            var subject = "Account Activation";
            var body = $"Hi {email},\n\nThe OTP code to activate your account is: {otp}\n\nBest Regard,\niLG Support";

            var smtpClient = new SmtpClient
            {
                Host = _appSettings.MailSettings.MailServer,
                Port = _appSettings.MailSettings.MailServerPort,
                EnableSsl = _appSettings.MailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_appSettings.MailSettings.MailServerUsername, _appSettings.MailSettings.MailServerPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
        }
    }
}