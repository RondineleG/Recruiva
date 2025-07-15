using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recruiva.Web.Data.Configurations;

public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("UserClaims", "Identity");

        builder.Property(c => c.ClaimType)
            .HasMaxLength(200);

        builder.Property(c => c.ClaimValue)
            .HasMaxLength(500);

        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("IX_UserClaims_UserId");
    }
}