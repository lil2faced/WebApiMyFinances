using System.ComponentModel.DataAnnotations;

namespace WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities
{
    public class User
    {
        [Key]
        public int Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
