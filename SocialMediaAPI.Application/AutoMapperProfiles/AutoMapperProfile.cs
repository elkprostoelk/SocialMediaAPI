using AutoMapper;
using SocialMediaAPI.Common.DTO;
using SocialMediaAPI.DataAccess.Entities;

namespace SocialMediaAPI.Application.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
