using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;
using WebApiMyFinances.Shared.Exceptions;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly ILogger<UserApiService> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public UserApiService(DatabaseContext databaseContext, IMapper mapper, IJwtProvider jwtProvider, ILogger<UserApiService> logger, IPasswordHasher passwordHasher)
        {
            _databaseContext = databaseContext;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
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

            // Сравниваем хеши паролей
            if (!_passwordHasher.VerifyHashedPassword(userApi.Password, user.Password))
                throw new NotAuthenticationException("Пользователь не аутентифицирован");
            
            var jwtToken = _jwtProvider.GenerateToken(_mapper.Map<DTOUserAPIJwt>(userApi));
             
            _logger.LogInformation($"Пользователь API {user.Email} вошел в систему, токен был сгенерирован");

            return jwtToken;
        }

        public async Task Register(DTOUserAPIRegister user, CancellationToken cancellationToken)
        {
            if (user is null)
                throw new ArgumentNullException("В качестве аргумента получен NULL");

            bool userExists = await _databaseContext.ApiUsers
                .AnyAsync(p => p.Email == user.Email, cancellationToken);

            if (userExists)
                throw new BadRequestException("Такой пользователь уже существует");

            var role = await _databaseContext.ApiRoles
                .FirstOrDefaultAsync(r => r.Role == user.RoleName, cancellationToken)
                ?? throw new NotFoundException($"Роль '{user.RoleName}' не найдена");

            // Хешируем пароль перед сохранением
            var hashedPassword = _passwordHasher.HashPassword(user.Password);

            var newUser = new UserAPI
            {
                Email = user.Email,
                Password = hashedPassword,
                RoleId = role.Id
            };

            await _databaseContext.ApiUsers.AddAsync(newUser, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Создан новый пользователь API: {user.Email} с ролью {user.RoleName}");
        }
    }
}