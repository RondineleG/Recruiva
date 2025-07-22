using Recruiva.Core.Entities.Base;
using Recruiva.Core.Validations;

namespace Recruiva.Core.Interfaces.Validations;

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