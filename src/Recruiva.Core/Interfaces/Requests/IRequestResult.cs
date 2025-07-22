using Recruiva.Core.Enums;

namespace Recruiva.Core.Interfaces.Requests;

public interface IRequestResult
{
    EResultStatus Status { get; }
}