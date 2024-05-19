using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserInfoService
    {
        Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, int minAge, int maxAge, string gender, int pageIndex, int pageSize);
    }
}