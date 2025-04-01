using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities;
using WebApiMyFinances.Shared.Exceptions;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<UserService> _logger;
        public UserService(IMapper mapper, DatabaseContext context, ILogger<UserService> logger)
        {
            _mapper = mapper;
            _databaseContext = context;
            _logger = logger;
        }

        public async Task DeleteUser(string email, CancellationToken cancellationToken)
        {
            if (email is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _databaseContext.Users.AnyAsync(p => p.Email == email, cancellationToken);

            if (!IsHave)
                throw new NotFoundException("Пользователь не найден");

            var temp = await _databaseContext.Users.FirstOrDefaultAsync(p => p.Email == email, cancellationToken)
                ??throw new NotFoundException("Пользователь не найден");
            _databaseContext.Users.Remove(temp);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Пользователь {email} был удален");
        }

        public async Task EditUser(string email, DTOUserEdit userEdit, CancellationToken cancellationToken)
        {
            if (email is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            User existingUser = await _databaseContext.Users
                .FirstOrDefaultAsync(p => p.Email == email, cancellationToken)??
                throw new NotFoundException("Пользователь не найден");

            _mapper.Map(userEdit, existingUser);

            await _databaseContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Пользователь {userEdit.Name} был обновлен");
        }

        public async Task<IEnumerable<DTOUserGet>> GetAllUsers(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userDTOs = await _databaseContext.Users
                .ProjectTo<DTOUserGet>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            _logger.LogInformation($"Запрос на получение списка всех пользователей");
            return userDTOs;
        }

        public async Task<DTOUserGet> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            var User = await _databaseContext.Users
                .Where(p => p.Email == email)
                .ProjectTo<DTOUserGet>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Пользователь не найден");
            _logger.LogInformation($"Запрос на получение пользователя с email {email}");
            return User;
        }

        public async Task SetUser(DTOUserSet userSet, CancellationToken cancellationToken)
        {
            if (userSet is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _databaseContext.Users.AnyAsync(p => p.Email == userSet.Email, cancellationToken);

            if (IsHave)
                throw new BadRequestException("Такой пользователь уже существует");

            User user = _mapper.Map<User>(userSet);

            await _databaseContext.AddAsync(user, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Пользователь {user.Name} || {user.Email} был добавлен");
        }
    }
}
