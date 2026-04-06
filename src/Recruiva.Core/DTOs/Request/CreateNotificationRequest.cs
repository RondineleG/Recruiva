using Recruiva.Core.Enums;

namespace Recruiva.Core.DTOs.Request;

public class CreateNotificationRequest
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
    public ENotificationType Type { get; set; } = ENotificationType.System;
}
