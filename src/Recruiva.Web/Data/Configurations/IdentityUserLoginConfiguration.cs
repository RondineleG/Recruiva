using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recruiva.Web.Data.Configurations;

public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogins", "Identity");

        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.Property(l => l.LoginProvider)
            .HasMaxLength(200);

        builder.Property(l => l.ProviderKey)
            .HasMaxLength(200);

        builder.Property(l => l.ProviderDisplayName)
            .HasMaxLength(200);

        builder.HasIndex(l => l.UserId)
            .HasDatabaseName("IX_UserLogins_UserId");
    }
}