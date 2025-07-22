using Recruiva.Core.Entities.Base;

namespace Recruiva.Core.Interfaces.Validations;

public interface IEntityValidator<in T>
    where T : BaseEntity
{
    IValidationResult Validate(T entity);

    IValidationResult ValidateUpdate(T entity);
}