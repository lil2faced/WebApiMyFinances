using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Core.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(DTOUserAPIJwt user);
    }
}
