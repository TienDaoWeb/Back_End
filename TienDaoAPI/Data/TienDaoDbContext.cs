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
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
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
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ReadChapter> ReadChapters { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}
