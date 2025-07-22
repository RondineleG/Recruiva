namespace Recruiva.Core.Interfaces.Requests;

public interface IRequestCustomResult<out T> : IRequestResult
{
    T? Data { get; }
}