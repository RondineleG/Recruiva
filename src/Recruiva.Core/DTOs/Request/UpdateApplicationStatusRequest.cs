using Recruiva.Core.Enums;

namespace Recruiva.Core.DTOs.Request;

public class UpdateApplicationStatusRequest
{
    public Guid ApplicationId { get; set; }
    public EApplicationStatus NewStatus { get; set; }
    public string? Notes { get; set; }
}
