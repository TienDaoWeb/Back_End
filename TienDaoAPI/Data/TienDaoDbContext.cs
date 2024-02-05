using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Models;

namespace TienDaoAPI.Data
{
    public class TienDaoDbContext : IdentityDbContext<User, Role, int>
    {
        public TienDaoDbContext(DbContextOptions<TienDaoDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users")
                                  .Ignore(u => u.TwoFactorEnabled)
                                  .Ignore(u => u.NormalizedUserName)
                                  .Ignore(u => u.NormalizedEmail)
                                  .Ignore(u => u.PhoneNumberConfirmed);
            builder.Entity<Role>().ToTable("Roles");
        }
        public DbSet<Story> Stories { get; set; }
    }

}
