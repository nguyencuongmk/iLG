using iLG.API.Helpers;
using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace iLG.API.Services
{
    public class EmailService(IOptions<AppSettings> options, IUserRepository userRepository) : IEmailService
    {
        private readonly AppSettings _appSettings = options.Value;
        private readonly IUserRepository _userRepository = userRepository;

        /// <summary>
        /// Request to Activate User Account
        /// </summary>
        /// <param name="email"></param>
        /// <param name="isResend"></param>
        /// <returns></returns>
        public async Task<bool> SendActivationEmail(string email, bool isResend = false)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (!EmailHelper.IsValidEmail(email))
                return false;

            if (!await _userRepository.IsExistAsync(expression: u => u.Email == email))
                return false;

            var user = await _userRepository.GetAsync(expression: u => u.Email == email);

            if (user is null)
                return false;

            if (isResend)
            {
                if (DateTime.UtcNow < user.OtpExpiredTime)
                    return false;

                user.Otp = OtpHelper.GenerateOTP();
                user.OtpExpiredTime = DateTime.UtcNow.AddMinutes(2);
                await _userRepository.UpdateAsync(user);
            }

            var otp = user?.Otp;

            if (string.IsNullOrEmpty(otp))
                return false;

            var subject = "Account Activation";
            var body = $@"<p>Hi {user?.UserInfo.FullName},</p>
                                <p>The OTP code to activate your account is: <b>{otp}</b></p>
                                <p>Best Regard,</p>
                                <p>iLG Support</p>";

            await Send(_appSettings.MailSettings.MailServerUsername, _appSettings.MailSettings.MailServerPassword, [email], [], subject, body);
            return true;
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