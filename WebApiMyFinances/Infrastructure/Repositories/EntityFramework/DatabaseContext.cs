using Microsoft.EntityFrameworkCore;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;

namespace WebApiMyFinances.Infrastructure.Repositories.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        private readonly string _ConnectionString;
        public DatabaseContext(IConfiguration configuration)
        {
            _ConnectionString = configuration["ConnectionStrings:DefaultConnection"]
                ?? throw new Exception("Строка подключения к бд не определена");

            Database.EnsureCreated();
        }

        public DbSet<UserAPI> ApiUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserApiRole> ApiRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_ConnectionString);
        }
    }
}
