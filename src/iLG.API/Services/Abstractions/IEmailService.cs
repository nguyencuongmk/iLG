namespace iLG.API.Services.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendActivationEmail(string email);
    }
}
