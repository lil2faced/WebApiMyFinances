using System.ComponentModel.DataAnnotations;

namespace WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security
{
    public class UserApiRole
    {
        [Key]
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public ICollection<UserAPI> ApiUsers { get; set; } = new List<UserAPI>();
    }
}
