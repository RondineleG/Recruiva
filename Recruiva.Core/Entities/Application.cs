using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Application : BaseEntity
{
    public DateTime? AppliedAt { get; set; }

    public virtual Candidate? Candidate { get; set; }

    [Required]
    public string CandidateId { get; set; } = string.Empty;

    public virtual Job? Job { get; set; }

    [Required]
    public string JobId { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? SelectedAt { get; set; }

    public EApplicationStatus Status { get; set; } = EApplicationStatus.Sent;

    public virtual ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();

    public DateTime? ViewedAt { get; set; }
}