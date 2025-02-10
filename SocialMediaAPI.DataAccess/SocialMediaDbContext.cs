using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.DataAccess.Entities;
using System.Reflection;

namespace SocialMediaAPI.DataAccess
{
    public class SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
