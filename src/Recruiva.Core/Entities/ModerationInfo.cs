using Recruiva.Core.Enums;

namespace Recruiva.Core.Entities;

public class ModerationInfo
{
    public DateTime? ModerationDate { get; set; }

    public string? ModeratorId { get; set; }

    public string? Reason { get; set; }

    public EModerationStatus Status { get; set; } = EModerationStatus.Pending;
}