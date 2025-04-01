using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;
using WebApiMyFinances.Shared.Exceptions;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Services
{
    public class UserApiRoleService : IUserApiRoleService
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _dbContext;
        public UserApiRoleService(IMapper mapper, DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
            _mapper = mapper;
        }
        public async Task DeleteRole(string role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _dbContext.ApiRoles.AnyAsync(p => p.Role == role, cancellationToken);

            if (!IsHave)
                throw new NotFoundException("Роль не найдена");

            var temp = await _dbContext.ApiRoles.FirstOrDefaultAsync(p => p.Role == role, cancellationToken)
                ?? throw new NotFoundException("Роль не найдена");
            _dbContext.ApiRoles.Remove(temp);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task EditRole(string roleToEdit, DTOUserApiRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role is null)
                throw new ArgumentNullException("На входе пришел NULL");

            UserApiRole existingUser = await _dbContext.ApiRoles
                .FirstOrDefaultAsync(p => p.Role == role.Role, cancellationToken) ??
                throw new NotFoundException("Роль не найдена");

            _mapper.Map(role, existingUser);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DTOUserApiRole>> GetAllRoles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _dbContext.ApiRoles
                .ProjectTo<DTOUserApiRole>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return roles;
        }

        public async Task SetRole(DTOUserApiRole roleSet, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (roleSet is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _dbContext.ApiRoles.AnyAsync(p => p.Role == roleSet.Role, cancellationToken);

            if (IsHave)
                throw new BadRequestException("Такая роль уже существует");

            UserApiRole role = _mapper.Map<UserApiRole>(roleSet);

            await _dbContext.AddAsync(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
