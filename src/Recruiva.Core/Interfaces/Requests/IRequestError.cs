using Recruiva.Core.Requests;

namespace Recruiva.Core.Interfaces.Requests;

public interface IRequestError : IRequestResult
{
    RequestError? RequestError { get; }
}