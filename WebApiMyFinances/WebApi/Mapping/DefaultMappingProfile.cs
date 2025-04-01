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
            CreateMap<UserAPI, DTOUserAPIRegister>()
                .ReverseMap();
            CreateMap<UserAPI, DTOUserApiLogin>()
                .ReverseMap();
            CreateMap<UserAPI, DTOUserAPIJwt>()
                .ReverseMap();
            CreateMap<User, DTOUserGet>()
                .ReverseMap();
            CreateMap<User, DTOUserSet>()
                .ReverseMap();
            CreateMap<User, DTOUserEdit>()
                .ReverseMap();
        }
    }
}
