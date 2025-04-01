using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;
using WebApiMyFinances.Security.Jwt;
using WebApiMyFinances.Shared.Exceptions;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public UserApiService(DatabaseContext databaseContext,IMapper mapper, IJwtProvider jwtProvider)
        {
            _databaseContext = databaseContext;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        } 

        public async Task<string> Login(DTOUserApiLogin user, CancellationToken cancellationToken)
        {
            if (user is null)
                throw new ArgumentNullException("В качестве аргумента получен NULL");

            var userApi = await _databaseContext.APIUsers.Where(p => p.Email == user.Email).FirstOrDefaultAsync()
                ?? throw new NotFoundException("Пользователь не найден");

            if (userApi.Password != user.Password)
                throw new NotAuthenticationException("Пользователь не аутентифицирован");

            var jwtToken = _jwtProvider.GenerateToken(_mapper.Map<DTOUserAPIJwt>(userApi));

            return jwtToken;
        }

        public async Task Register(DTOUserAPIRegister user, CancellationToken cancellationToken)
        {
            if (user is null)
                throw new ArgumentNullException("В качестве аргумента получен NULL");

            bool IsHave = await _databaseContext.APIUsers.AnyAsync(p => p.Email == user.Email);

            if (IsHave)
                throw new BadRequestException("Такой пользователь уже существует");

            UserAPI u = _mapper.Map<UserAPI>(user);

            await _databaseContext.APIUsers.AddAsync(u);
            await _databaseContext.SaveChangesAsync();
        }
    }
}