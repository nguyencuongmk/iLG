using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserInfoService
    {
        Task<(UserInfoResponse, string)> GetUserInfo(int id);

        Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, UserSuitableRequest request);
    }
}