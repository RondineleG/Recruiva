using Recruiva.Core.Entities.Base;
using Recruiva.Core.Enums;
using Recruiva.Core.ValueObjects;

using System.ComponentModel.DataAnnotations;

namespace Recruiva.Core.Entities;

public class Application : BaseEntity
{
    public DateTime? AppliedAt { get; set; }

    public virtual Candidate? Candidate { get; set; }

    [Required]
    public Id CandidateId { get; set; }

    public virtual Job? Job { get; set; }

    [Required]
    public Id JobId { get; set; }

    public string? Notes { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? SelectedAt { get; set; }

    public EApplicationStatus Status { get; set; } = EApplicationStatus.Sent;

    public virtual ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = [];

    public DateTime? ViewedAt { get; set; }
}