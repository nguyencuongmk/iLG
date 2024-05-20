using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserInfoService
    {
        Task<(UserInfoResponse, string)> GetUserInfo(int id);

        Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, int minAge, int maxAge, string gender, int pageIndex, int pageSize);
    }
}