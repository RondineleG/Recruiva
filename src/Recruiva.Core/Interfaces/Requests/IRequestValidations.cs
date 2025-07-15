using Recruiva.Web.Requests;

namespace Recruiva.Web.Interfaces.Requests;

public interface IRequestValidations : IRequestResult
{
    IEnumerable<RequestValidation> Validations { get; }
}