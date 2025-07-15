namespace Recruiva.Web.Entities;

public class ApplicationStatusHistory
{
    public Application Application { get; set; } = default!;

    public string ApplicationId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Note { get; set; }

    public string? Responsible { get; set; }

    public EApplicationStatus Status { get; set; }
}