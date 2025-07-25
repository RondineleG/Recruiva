﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Core.Entities;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Data.Configurations;

public class TenantConfigConfiguration : IEntityTypeConfiguration<TenantConfig>
{
    public void Configure(EntityTypeBuilder<TenantConfig> builder)
    {
        builder.ToTable("TenantConfigs");
        builder.HasKey(a => a.Id);
        builder.Property(e => e.Id)
     .HasConversion(
         id => id.Value,
         value => Id.Create(value)
     )
     .HasColumnType("UNIQUEIDENTIFIER");
        builder.Property(t => t.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.BaseUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.Property(t => t.Settings)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(t => t.BaseUrl)
            .IsUnique()
            .HasDatabaseName("IX_TenantConfigs_BaseUrl");

        builder.HasIndex(t => t.IsActive)
            .HasDatabaseName("IX_TenantConfigs_IsActive");
    }
}