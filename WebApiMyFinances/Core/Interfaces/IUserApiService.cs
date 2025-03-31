using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Interfaces
{
    public interface IUserApiService
    {
        Task Register(DTOUserAPIRegister user, CancellationToken cancellationToken);
        Task<string> Login(DTOUserApiLogin user, CancellationToken cancellationToken);
    }
}
