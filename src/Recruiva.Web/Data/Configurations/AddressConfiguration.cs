using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Web.Entities;

namespace Recruiva.Web.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.Property(a => a.City).HasMaxLength(100);
        builder.Property(a => a.Complement).HasMaxLength(100);
        builder.Property(a => a.Country).HasMaxLength(5).HasDefaultValue("BR").IsFixedLength();
        builder.Property(a => a.District).HasMaxLength(100);
        builder.Property(a => a.Number).HasMaxLength(20);
        builder.Property(a => a.State).HasMaxLength(5);
        builder.Property(a => a.Street).HasMaxLength(200);
        builder.Property(a => a.ZipCode).HasMaxLength(20);
    }
}