using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserService
    {
        Task<(LoginResponse, string)> Login(LoginRequest request);

        Task<string> Logout(int userId, string token);

        Task<string> Register(RegisterRequest request);

        Task<bool> VerifyToken(string token);

        Task<string> Activation(ActivationRequest request);

        Task<string> ChangePassword(ChangePasswordRequest request, string email);

        Task<(string, string)> ForgotPassword(ForgotPasswordRequest request);
    }
}
