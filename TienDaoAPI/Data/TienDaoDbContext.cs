using Microsoft.AspNetCore.Identity;
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
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            var roles = new List<Role>()
            {
                new Role(){
                        Id = 1,
                        ConcurrencyStamp = "AdminRole",
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper()
                },
                new Role(){
                        Id = 2,
                        ConcurrencyStamp = "ConverterRole",
                        Name = "Converter",
                        NormalizedName = "Converter".ToUpper()
                },
                new Role(){
                        Id = 3,
                        ConcurrencyStamp = "ReaderRole",
                        Name = "Reader",
                        NormalizedName = "Reader".ToUpper()
                }
            };

            builder.Entity<Role>().HasData(roles);

        }
        public DbSet<Story> Stories { get; set; }
    }

}
