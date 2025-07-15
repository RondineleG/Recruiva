using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Job : BaseEntity
{
    public virtual Advertiser? Advertiser { get; set; }

    [Required]
    public Id AdvertiserId { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = [];

    public string? Benefits { get; set; }

    public JobBoost? Boost { get; set; }

    public string? Category { get; set; }

    public JobCounters Counters { get; set; } = new();

    [Required]
    public string Description { get; set; } = string.Empty;

    public DateTime ExpirationDate { get; set; }

    public JobHighlight? Highlight { get; set; }

    public JobLocation? Location { get; set; }

    public ModerationInfo? Moderation { get; set; }

    public string? Requirements { get; set; }

    public string? Responsibilities { get; set; }

    public SalaryRange? Salary { get; set; }

    public EJobStatus Status { get; set; } = EJobStatus.Active;

    public string? Tags { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}