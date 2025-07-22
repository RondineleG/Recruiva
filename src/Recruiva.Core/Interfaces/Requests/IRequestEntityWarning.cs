using Recruiva.Core.Requests;

namespace Recruiva.Core.Interfaces.Requests;

public interface IRequestEntityWarning : IRequestResult
{
    RequestEntityWarning? RequestEntityWarning { get; }
}