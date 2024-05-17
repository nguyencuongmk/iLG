using iLG.API.Constants;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.API.Services
{
    public class UserInfoService(IUserInfoRepository userInfoRepository) : IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository = userInfoRepository;

        public async Task<(SearchSuitableResponse, string)> SearchSuitableUser(SearchSuitableRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var searchResponse = new SearchSuitableResponse();

            if (request == null || string.IsNullOrEmpty(request?.Gender))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return (searchResponse, message);
            }

            var userInfo = await _userInfoRepository.GetAsync(ui => ui.UserId == request.UserId);

            if (userInfo is null)
            {
                message = Message.Error.User.NOT_EXISTS_USER;
                return (searchResponse, message);
            }

            #endregion Verify Request

            #region Business Logic

            var hobbies = new List<string>();
            userInfo.Hobbies.ForEach(x =>
            {
                if (x is not null)
                {
                    if (!string.IsNullOrEmpty(x.Title))
                        hobbies.Add(x.Title);

                    if (x.HobbyDetails.Count != 0)
                    {
                        // todo
                    }
                }
                    
            });

            #endregion Business Logic

            throw new NotImplementedException();
        }
    }
}