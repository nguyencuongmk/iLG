using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserService
    {
        Task<(LoginResponse, string)> SignIn(SigninRequest request);

        Task<string> SignOut(int userId, string? token);

        Task<string> SignUp(SignupRequest request);

        Task<bool> VerifyToken(string token);

        Task<string> ChangePassword(ChangePasswordRequest request, string? email);

        Task<(string, string)> ForgotPassword(ForgotPasswordRequest request);
    }
}
