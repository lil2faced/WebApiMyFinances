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

            cancellationToken.ThrowIfCancellationRequested();

            var userApi = await _databaseContext.ApiUsers
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken)
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

            // Проверяем существование пользователя
            bool userExists = await _databaseContext.ApiUsers
                .AnyAsync(p => p.Email == user.Email, cancellationToken);

            if (userExists)
                throw new BadRequestException("Такой пользователь уже существует");

            // Ищем роль в базе данных
            var role = await _databaseContext.ApiRoles
                .FirstOrDefaultAsync(r => r.Role == user.RoleName, cancellationToken)
                ?? throw new NotFoundException($"Роль '{user.RoleName}' не найдена");

            // Создаем пользователя
            var newUser = new UserAPI
            {
                Email = user.Email,
                Password = user.Password,
                RoleId = role.Id // Устанавливаем связь через RoleId
            };

            await _databaseContext.ApiUsers.AddAsync(newUser, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}