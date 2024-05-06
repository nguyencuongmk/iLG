using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;

namespace iLG.API.Services
{
    public class UserService : IUserService
    {
        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyToken(string token)
        {
            return true;
        }
    }
}
