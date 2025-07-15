using Recruiva.Web.Enums;

using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Notification : BaseEntity
{
    public bool IsRead { get; set; } = false;

    public string? Message { get; set; }

    public DateTime? ReadAt { get; set; }

    [Required]
    public string RecipientId { get; set; } = string.Empty;

    public string? Title { get; set; }

    public ENotificationType Type { get; set; } = ENotificationType.System;
}