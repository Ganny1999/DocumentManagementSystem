using AutoMapper;
using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;

namespace DocumentIdentityService.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User,UserDto>().ReverseMap();
        }
    }
}
