using System.ComponentModel.DataAnnotations;

namespace Recruiva.Core.DTOs.Request;

public class CancelSubscriptionRequest
{
    [Required]
    public Guid SubscriptionId { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}
