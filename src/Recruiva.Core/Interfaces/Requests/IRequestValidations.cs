using Recruiva.Core.Requests;

namespace Recruiva.Core.Interfaces.Requests;

public interface IRequestValidations : IRequestResult
{
    IEnumerable<RequestValidation> Validations { get; }
}