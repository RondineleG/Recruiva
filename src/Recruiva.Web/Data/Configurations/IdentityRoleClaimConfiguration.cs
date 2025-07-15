using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recruiva.Web.Data.Configurations;

public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        builder.ToTable("RoleClaims", "Identity");

        builder.Property(rc => rc.ClaimType)
            .HasMaxLength(200);

        builder.Property(rc => rc.ClaimValue)
            .HasMaxLength(500);

        builder.HasIndex(rc => rc.RoleId)
            .HasDatabaseName("IX_RoleClaims_RoleId");
    }
}