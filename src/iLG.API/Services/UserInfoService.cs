using AutoMapper;
using iLG.API.Constants;
using iLG.API.Models.Requests;
using iLG.API.Models.Responses;
using iLG.API.Services.Abstractions;
using iLG.Domain.Enums;
using iLG.Infrastructure.Extentions;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.API.Services
{
    public class UserInfoService(IUserInfoRepository userInfoRepository, IUserRepository userRepository, IMapper mapper) : IUserInfoService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserInfoRepository _userInfoRepository = userInfoRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get user info by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(UserInfoResponse, string)> GetUserInfo(int id)
        {   
            string message = string.Empty;
            var user = await _userRepository.GetAsync(expression: u => u.Id == id && !u.IsLocked && !u.IsDeleted);
            var userInfoResponse = new UserInfoResponse();

            if (user is null || user.UserInfo is null)
                message = Message.Error.UserInfo.NOT_EXISTS;
            else
            {
                var userInfo = user.UserInfo;
                userInfoResponse = _mapper.Map<UserInfoResponse>(userInfo);
                userInfoResponse.Relationship = userInfo.Relationship.Title;
                userInfoResponse.Company = userInfo.Company.Title;
                userInfoResponse.Job = userInfo.Job.Title;
                userInfoResponse.Height = userInfo.Height;
            }

            return (userInfoResponse, message);
        }

        /// <summary>
        /// Search suitable user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, UserSuitableRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var userSuitables = new List<UserSuitableResponse>();

            if (string.IsNullOrEmpty(request.Gender))
            {
                message = Message.Error.User.NOT_ENOUGH_INFO;
                return (userSuitables, message);
            }

            if (request.PageIndex < 1 || request.PageSize < 0)
            {
                message = Message.Error.Paging.INVALID_PAGING;
                return (userSuitables, message);
            }

            var user = await _userRepository.GetAsync(expression: u => u.Id == userId && !u.IsLocked && !u.IsDeleted);

            if (user is null || user.UserInfo is null)
                return (userSuitables, Message.Error.User.NOT_EXISTS_USER);       

            #endregion Verify Request

            #region Business Logic

            var userInfos = await _userInfoRepository.GetListAsync(expression: ui => DateTime.UtcNow.Year - ui.DateOfBirth.Year >= request.MinAge && DateTime.UtcNow.Year - ui.DateOfBirth.Year <= request.MaxAge && ui.Gender == request.Gender.ToEnum<Gender>() && ui.UserId != userId && !ui.User.IsLocked && !ui.User.IsDeleted);

            if (userInfos.Any())
            {
                var userInfo = user.UserInfo;

                userSuitables = userInfos.Where(ui => ui.Hobbies.Intersect(userInfo.Hobbies).Any()).Select(ui => new UserSuitableResponse
                {
                    UserId = ui.UserId,
                    Relationship = ui.Relationship.Title,
                    Company = ui.Company.Title,
                    Job = ui.Job.Title,
                    FullName = ui.FullName,
                    Age = DateTime.UtcNow.Year - ui.DateOfBirth.Year,
                    Gender = ui.Gender.ToString(),
                    Nickname = ui.Nickname,
                    PhoneNumber = ui.PhoneNumber,
                    Height = ui.Height,
                    Zodiac = ui.Zodiac.ToString(),
                    Biography = ui.Biography,
                    Images = _mapper.Map<List<ImageResponse>>(ui.Images),
                    Hobbies = _mapper.Map<List<HobbyResponse>>(ui.Hobbies),
                    SameHobbies = ui.Hobbies.Intersect(userInfo.Hobbies).Select(x => x.Name).ToList()
                }).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
            }

            return (userSuitables, message);

            #endregion Business Logic

        }
    }
}