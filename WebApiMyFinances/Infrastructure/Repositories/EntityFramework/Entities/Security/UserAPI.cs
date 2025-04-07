using System.ComponentModel.DataAnnotations;

namespace WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security
{
    public class UserAPI
    {
        [Key]
        public Guid Id { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public UserApiRole Role { get; set; } = null!;
    }
}
