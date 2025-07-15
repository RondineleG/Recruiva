using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Recruiva.Web.Entities;

namespace Recruiva.Web.Data.Configurations;

public class ResumeSkillConfiguration : IEntityTypeConfiguration<ResumeSkill>
{
    public void Configure(EntityTypeBuilder<ResumeSkill> builder)
    {
        builder.ToTable("ResumeSkills");

        builder.HasKey(rs => new { rs.ResumeId, rs.Skill });

        builder.Property(rs => rs.Skill)
            .HasMaxLength(100);

        builder.Property(rs => rs.Level)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(rs => rs.YearsOfExperience)
            .HasDefaultValue(0);

        builder.HasOne(rs => rs.Resume)
            .WithMany(r => r.Skills)
            .HasForeignKey(rs => rs.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rs => rs.Skill)
            .HasDatabaseName("IX_ResumeSkills_Skill");

        builder.HasIndex(rs => new { rs.Skill, rs.Level })
            .HasDatabaseName("IX_ResumeSkills_Skill_Level");
    }
}