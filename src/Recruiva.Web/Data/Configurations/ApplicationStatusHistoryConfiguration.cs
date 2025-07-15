using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Web.Entities;

namespace Recruiva.Web.Data.Configurations;

public class ApplicationStatusHistoryConfiguration : IEntityTypeConfiguration<ApplicationStatusHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusHistory> builder)
    {
        builder.ToTable("ApplicationStatusHistory");

        builder.HasKey(h => new { h.ApplicationId, h.Date });

        builder.Property(h => h.Responsible)
            .HasMaxLength(100);

        builder.Property(h => h.Note)
            .HasMaxLength(1000);

        builder.Property(h => h.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(h => h.Date)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne<Application>()
            .WithMany()
            .HasForeignKey(h => h.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.ApplicationId)
            .HasDatabaseName("IX_ApplicationStatusHistory_ApplicationId");
    }
}