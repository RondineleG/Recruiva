using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recruiva.Web.Data.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("Roles", "Identity");

        builder.Property(r => r.Name)
            .HasMaxLength(256);

        builder.Property(r => r.NormalizedName)
            .HasMaxLength(256);

        builder.HasIndex(r => r.NormalizedName)
            .IsUnique()
            .HasDatabaseName("IX_Roles_NormalizedName")
            .HasFilter("[NormalizedName] IS NOT NULL");
    }
}