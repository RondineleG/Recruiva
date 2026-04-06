using Recruiva.Core.Entities;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;

namespace Recruiva.Core.Interfaces.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    Task<RequestResult<IEnumerable<Job>>> SearchAsync(
        string? searchTerm = null,
        string? city = null,
        string? state = null,
        bool? isRemote = null,
        decimal? salaryMin = null,
        decimal? salaryMax = null,
        string? category = null,
        int page = 1,
        int size = 10);
}
