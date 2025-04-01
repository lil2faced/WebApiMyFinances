using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Interfaces
{
    public interface IUserApiRoleService
    {
        Task<IEnumerable<DTOUserApiRole>> GetAllRoles(CancellationToken cancellationToken);
        Task SetRole(DTOUserApiRole userSet, CancellationToken cancellationToken);
        Task EditRole(string role, DTOUserApiRole userEdit, CancellationToken cancellationToken);
        Task DeleteRole(string role, CancellationToken cancellationToken);
    }
}
