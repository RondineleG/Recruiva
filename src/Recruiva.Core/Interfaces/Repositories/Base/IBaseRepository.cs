using Recruiva.Core.Entities.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<RequestResult<TEntity>> CreateAsync(TEntity entity);

    Task<RequestResult<TEntity>> DeleteAsync(Id id);

    Task<RequestResult<IEnumerable<TEntity>>> GetAllAsync(int page, int size);

    Task<RequestResult<TEntity>> GetByIdAsync(Id id);

    Task<RequestResult<TEntity>> UpdateAsync(TEntity entity);
}