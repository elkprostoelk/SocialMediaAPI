using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaAPI.DataAccess.Entities;

namespace SocialMediaAPI.DataAccess.DbConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "User" }
                );
        }
    }
}
