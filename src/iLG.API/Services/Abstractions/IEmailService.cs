namespace iLG.API.Services.Abstractions
{
    public interface IEmailService
    {
        Task<string> SendOtpEmail(string email);

        Task<bool> SendNewPasswordEmail(string email, string password);
    }
}
