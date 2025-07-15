namespace Recruiva.Web.Interfaces.UseCases;

public interface IUseCase<TRequest, TResponse>
{
    Task<RequestResult<TResponse>> ExecuteAsync(TRequest request);
}