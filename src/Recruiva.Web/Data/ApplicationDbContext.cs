using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Recruiva.Web.Data.Extensions;
using Recruiva.Web.Models;

namespace Recruiva.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
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
    }
}