using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Interfaces
{
    public interface IUserService
    {
        Task<DTOUserGet> GetUserByEmail(string email, CancellationToken cancellationToken);
        Task<IEnumerable<DTOUserGet>> GetAllUsers(CancellationToken cancellationToken);
        Task SetUser(DTOUserSet userSet, CancellationToken cancellationToken);
        Task EditUser(string email, DTOUserEdit userEdit, CancellationToken cancellationToken);
        Task DeleteUser(string email, CancellationToken cancellationToken);
    }
}
