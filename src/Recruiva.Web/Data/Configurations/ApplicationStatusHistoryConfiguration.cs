using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Web.ValueObjects;

namespace Recruiva.Web.Data.Configurations;

public class ApplicationStatusHistoryConfiguration : IEntityTypeConfiguration<ApplicationStatusHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusHistory> builder)
    {
        builder.ToTable("ApplicationStatusHistory");

        builder.HasKey(a => a.Id);

        builder.Property(e => e.Id)
  .HasConversion(
      id => id.Value,
      value => Id.Create(value)
  )
  .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(h => h.ApplicationId)
            .HasConversion(
                 id => id.Value,
      value => Id.Create(value)
            )
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(h => h.Responsible)
            .HasMaxLength(100);

        builder.Property(h => h.Note)
            .HasMaxLength(1000);

        builder.Property(h => h.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(h => h.Date)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(h => h.Application)
            .WithMany(a => a.StatusHistory)
            .HasForeignKey(h => h.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.ApplicationId)
            .HasDatabaseName("IX_ApplicationStatusHistory_ApplicationId");
    }
}