using AutoMapper;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.WebApi.Mapping
{
    public class DefaultMappingProfile : Profile 
    {
        public DefaultMappingProfile()
        {
            CreateMap<DTOUserAPIRegister, UserAPI>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());
            CreateMap<UserAPI, DTOUserApiLogin>()
                .ReverseMap();
            CreateMap<User, DTOUserGet>()
                .ReverseMap();
            CreateMap<User, DTOUserSet>()
                .ReverseMap();
            CreateMap<User, DTOUserEdit>()
                .ReverseMap();
            CreateMap<UserApiRole, DTOUserApiRole>()
                .ReverseMap();
            CreateMap<UserAPI, DTOUserAPIJwt>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
