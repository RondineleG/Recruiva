namespace Recruiva.Web.Interfaces.Requests;

public interface IRequestCustomResult<out T> : IRequestResult
{
    T? Data { get; }
}