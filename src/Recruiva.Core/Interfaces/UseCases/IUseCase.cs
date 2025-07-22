using Recruiva.Core.Requests;

namespace Recruiva.Core.Interfaces.UseCases;

public interface IUseCase<TRequest, TResponse>
{
    Task<RequestResult<TResponse>> ExecuteAsync(TRequest request);
}