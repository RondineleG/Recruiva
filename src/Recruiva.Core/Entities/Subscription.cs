using Recruiva.Core.Entities.Base;
using Recruiva.Core.Enums;
using Recruiva.Core.ValueObjects;

using System.ComponentModel.DataAnnotations;

namespace Recruiva.Core.Entities;

public class Subscription : BaseEntity
{
    public Subscription()
    {
        InitializeNewEntity();
    }

    [Required]
    public Id AdvertiserId { get; set; } = Id.Empty;

    public virtual Advertiser? Advertiser { get; set; }

    [Required]
    public Id PlanId { get; set; } = Id.Empty;

    public virtual SubscriptionPlan? Plan { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }

    [Required]
    public ESubscriptionStatus Status { get; set; } = ESubscriptionStatus.Pending;

    /// <summary>
    /// ID do gateway de pagamento (referência externa).
    /// </summary>
    [MaxLength(100)]
    public string? PaymentId { get; set; }

    /// <summary>
    /// Motivo do cancelamento (quando aplicável).
    /// </summary>
    [MaxLength(500)]
    public string? CancellationReason { get; set; }
}
