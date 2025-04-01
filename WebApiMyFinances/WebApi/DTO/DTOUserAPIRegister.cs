

namespace WebApiMyFinances.WebApi.DTO
{
    public class DTOUserAPIRegister
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = "User";
    }
}
