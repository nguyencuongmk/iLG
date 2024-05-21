using iLG.API.Models.Requests;

namespace iLG.API.Services.Abstractions
{
    public interface IEmailService
    {
        Task<string> SendOtpEmail(SendOtpRequest request);

        Task<bool> SendNewPasswordEmail(string email, string password);
    }
}
