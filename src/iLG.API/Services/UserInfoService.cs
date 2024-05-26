﻿using AutoMapper;
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

        /// <summary>
        /// Get user info by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(UserInfoResponse, string)> GetUserInfo(int id)
        {   
            string message = string.Empty;
            var userInfo = await _userInfoRepository.GetAsync(expression: ui => ui.UserId == id);
            var userInfoResponse = new UserInfoResponse();

            if (userInfo is null)
                message = Message.Error.UserInfo.NOT_EXISTS;
            else
            {
                userInfoResponse = _mapper.Map<UserInfoResponse>(userInfo);
                userInfoResponse.Relationship = userInfo.Relationship.Title;
                userInfoResponse.Company = userInfo.Company.Title;
                userInfoResponse.Job = userInfo.Job.Title;
            }

            return (userInfoResponse, message);
        }

        /// <summary>
        /// Search suitable user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="minAge"></param>
        /// <param name="maxAge"></param>
        /// <param name="gender"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<UserSuitableResponse>, string)> SearchSuitableUser(int userId, int minAge, int maxAge, string gender, int pageIndex, int pageSize)
        {
            #region Verify Request

            var message = string.Empty;
            var userSuitables = new List<UserSuitableResponse>();

            if (string.IsNullOrEmpty(gender))
            {
                message = Message.Error.Account.NOT_ENOUGH_INFO;
                return (userSuitables, message);
            }

            if (pageIndex < 1 || pageSize < 0)
            {
                message = Message.Error.Paging.INVALID_PAGING;
                return (userSuitables, message);
            }

            var userInfo = await _userInfoRepository.GetAsync(ui => ui.UserId == userId);

            if (userInfo is null)
            {
                message = Message.Error.Account.NOT_EXISTS_USER;
                return (userSuitables, message);
            }

            #endregion Verify Request

            #region Business Logic

            var userInfos = await _userInfoRepository.GetListAsync(expression: ui => DateTime.UtcNow.Year - ui.DateOfBirth.Year >= minAge && DateTime.UtcNow.Year - ui.DateOfBirth.Year <= maxAge && ui.Gender == gender.ToEnum<Gender>() && ui.UserId != userId);

            if (userInfos.Any())
            {
                userSuitables = userInfos.Where(ui => ui.Hobbies.Intersect(userInfo.Hobbies).Any()).Select(ui => new UserSuitableResponse
                {
                    UserId = ui.UserId,
                    FullName = ui.FullName,
                    Age = DateTime.UtcNow.Year - ui.DateOfBirth.Year,
                    Gender = ui.Gender.ToString(),
                    Nickname = ui.Nickname,
                    PhoneNumber = ui.PhoneNumber,
                    Zodiac = ui.Zodiac.ToString(),
                    Biography = ui.Biography,
                    Images = _mapper.Map<List<ImageResponse>>(ui.Images),
                    Hobbies = _mapper.Map<List<HobbyResponse>>(ui.Hobbies),
                    SameHobbies = ui.Hobbies.Intersect(userInfo.Hobbies).Select(x => x.Name).ToList()
                }).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }

            return (userSuitables, message);

            #endregion Business Logic

        }
    }
}