using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaAPI.DataAccess.Entities;

namespace SocialMediaAPI.DataAccess.DbConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.PhoneCode)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
