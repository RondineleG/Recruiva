using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Recruiva.Web.Converters;
using Recruiva.Web.Data.Extensions;
using Recruiva.Web.ValueObjects;

namespace Recruiva.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext() : base(new DbContextOptions<ApplicationDbContext>())
    {
    }

    public DbSet<Advertiser> Advertisers => Set<Advertiser>();

    public DbSet<Application> Applications => Set<Application>();

    public DbSet<Candidate> Candidates => Set<Candidate>();

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<Resume> Resumes => Set<Resume>();

    public DbSet<TenantConfig> TenantConfigs => Set<TenantConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<JobBoost>();
        modelBuilder.Ignore<JobHighlight>();
        modelBuilder.Ignore<JobLocation>();
        modelBuilder.Ignore<SalaryRange>();
        modelBuilder.Ignore<ModerationInfo>();
        modelBuilder.Ignore<JobCounters>();
        modelBuilder.Ignore<Education>();
        modelBuilder.Ignore<Experience>();
        modelBuilder.Ignore<Language>();

        modelBuilder.ApplyEntityConfigurations();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(Id) || property.ClrType == typeof(Id))
                {
                    property.SetValueConverter(new IdValueConverter());
                    property.SetValueComparer(new IdValueComparer());
                }
            }
        }
    }
}