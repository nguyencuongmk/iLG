using AutoMapper;
using iLG.API.Models.Responses;
using iLG.Domain.Entities;

namespace iLG.API.Maps
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Hobby, HobbyResponse>();
            CreateMap<HobbyDetail, HobbyDetailResponse>();
            CreateMap<Image, ImageResponse>().ReverseMap();
            CreateMap<UserInfo, UserInfoResponse>().ReverseMap();
        }
    }
}