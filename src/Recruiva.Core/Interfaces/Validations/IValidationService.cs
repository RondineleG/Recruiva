using Recruiva.Web.Entities.Base;
using Recruiva.Web.Validations;

namespace Recruiva.Web.Interfaces.Validations;

public interface IValidationService<T>
    where T : BaseEntity
{
    IValidationBuilder<T> Builder { get; }

    IEntityValidator<T> Validator { get; }

    IValidationService<T> AddRule(IValidationRule<T> rule);

    IValidationService<T> AddValidation(Func<T, ValidationResult> validation);

    IValidationResult Validate(T entity);

    IValidationResult ValidateUpdate(T entity);

    IValidationService<T> WithErrorHandler(Action<ValidationErrorMessage> errorHandler);
}