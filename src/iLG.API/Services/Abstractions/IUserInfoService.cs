using iLG.API.Models.Requests;
using iLG.API.Models.Responses;

namespace iLG.API.Services.Abstractions
{
    public interface IUserInfoService
    {
        Task<(SearchSuitableResponse, string)> SearchSuitableUser(SearchSuitableRequest request);
    }
}