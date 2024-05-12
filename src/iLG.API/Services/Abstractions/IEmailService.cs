namespace iLG.API.Services.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendActivationEmail(string email, bool isResend = false);

        Task<bool> SendNewPasswordEmail(string email, string password);
    }
}
