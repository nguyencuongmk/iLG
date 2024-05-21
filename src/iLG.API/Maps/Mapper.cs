using AutoMapper;
using iLG.API.Models.Responses;
using iLG.Domain.Entities;

namespace iLG.API.Maps
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<HobbyCategory, HobbyCategoryResponse>().ReverseMap();
            CreateMap<Hobby, HobbyResponse>().ReverseMap();
            CreateMap<Image, ImageResponse>().ReverseMap();
            CreateMap<UserInfo, UserInfoResponse>().ReverseMap();
        }
    }
}