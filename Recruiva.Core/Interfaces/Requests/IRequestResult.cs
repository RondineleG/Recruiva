using Recruiva.Web.Enums;

namespace Recruiva.Web.Interfaces.Requests;

public interface IRequestResult
{
    EResultStatus Status { get; }
}