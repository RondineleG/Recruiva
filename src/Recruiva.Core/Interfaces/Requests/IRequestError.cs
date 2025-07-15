namespace Recruiva.Web.Interfaces.Requests;

public interface IRequestError : IRequestResult
{
    RequestError? RequestError { get; }
}