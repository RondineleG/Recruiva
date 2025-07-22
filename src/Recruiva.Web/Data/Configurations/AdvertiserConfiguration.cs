using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Core.Entities;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Data.Configurations;

public class AdvertiserConfiguration : IEntityTypeConfiguration<Advertiser>
{
    public void Configure(EntityTypeBuilder<Advertiser> builder)
    {
        builder.ToTable("Advertisers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => Id.Create(value)
            )
            .HasColumnType("uniqueidentifier");

        builder.Property(a => a.AddressId)
            .HasConversion(
                id => id.Value,
                value => Id.Create(value)
            )
            .HasColumnType("uniqueidentifier");

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Phone)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(a => a.TaxId)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(a => a.Email)
            .IsUnique()
            .HasDatabaseName("IX_Advertisers_Email");

        builder.HasIndex(a => a.TaxId)
            .IsUnique()
            .HasDatabaseName("IX_Advertisers_TaxId");

        builder.HasIndex(a => a.Status)
            .HasDatabaseName("IX_Advertisers_Status");

        builder.HasOne(a => a.Address)
            .WithOne()
            .HasForeignKey<Advertiser>(a => a.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}