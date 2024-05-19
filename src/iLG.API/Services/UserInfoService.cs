using AutoMapper;
using iLG.API.Constants;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Domain.Enums;
using iLG.Infrastructure.Extentions;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.API.Services
{
    public class UserInfoService(IUserInfoRepository userInfoRepository, IMapper mapper) : IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository = userInfoRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, int minAge, int maxAge, string gender, int pageIndex, int pageSize)
        {
            #region Verify Request

            var message = string.Empty;
            var searchResponse = new List<UserSuitableResponse>();

            if (string.IsNullOrEmpty(gender))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return (searchResponse, message);
            }

            var userInfo = await _userInfoRepository.GetAsync(ui => ui.UserId == userId);

            if (userInfo is null)
            {
                message = Message.Error.User.NOT_EXISTS_USER;
                return (searchResponse, message);
            }

            #endregion Verify Request

            #region Business Logic

            var userInfos = await _userInfoRepository.GetListAsync(expression: ui => ui.Age >= minAge && ui.Age <= maxAge && ui.Gender == gender.ToEnum<Gender>() && ui.UserId != userId);

            var userSuitables = userInfos.Where(ui => ui.HobbyDetails.Intersect(userInfo.HobbyDetails).Any()).Select(ui => new UserSuitableResponse
                    {
                        FullName = ui.FullName,
                        Age = ui.Age,
                        Gender = ui.Gender.ToString(),
                        Nickname = ui.Nickname,
                        PhoneNumber = ui.PhoneNumber,
                        RelationshipStatus = ui.RelationshipStatus,
                        Zodiac = ui.Zodiac,
                        Biography = ui.Biography,
                        Images = _mapper.Map<List<ImageResponse>>(ui.Images),
                        SameHobbies = ui.HobbyDetails.Intersect(userInfo.HobbyDetails).Select(x => x.Name).ToList()
                    }).ToList();

            return userSuitables.Count != 0 ? (userSuitables, message) : (searchResponse, message);

            #endregion Business Logic       
        }
    }
}