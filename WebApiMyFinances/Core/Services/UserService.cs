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
        public UserService(IMapper mapper, DatabaseContext context)
        {
            _mapper = mapper;
            _databaseContext = context;
        }

        public async Task DeleteUser(string email, CancellationToken cancellationToken)
        {
            if (email is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _databaseContext.Users.AnyAsync(p => p.Email == email);

            if (!IsHave)
                throw new NotFoundException("Пользователь не найден");

            var temp = await _databaseContext.Users.FirstOrDefaultAsync(p => p.Email == email)
                ??throw new NotFoundException("Пользователь не найден");
            _databaseContext.Users.Remove(temp);
            await _databaseContext.SaveChangesAsync(cancellationToken);
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
        }

        public async Task<IEnumerable<DTOUserGet>> GetAllUsers(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userDTOs = await _databaseContext.Users
                .ProjectTo<DTOUserGet>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

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

            return User;
        }

        public async Task SetUser(DTOUserSet userSet, CancellationToken cancellationToken)
        {
            if (userSet is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _databaseContext.Users.AnyAsync(p => p.Email == userSet.Email);

            if (IsHave)
                throw new BadRequestException("Такой пользователь уже существует");

            User user = _mapper.Map<User>(userSet);

            await _databaseContext.AddAsync(user);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
