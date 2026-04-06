using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Applications;

public class ListApplicationsByJobUseCase : IUseCase<Guid, IEnumerable<ApplicationResponse>>
{
    private readonly IBaseRepository<Application> _applicationRepository;

    public ListApplicationsByJobUseCase(IBaseRepository<Application> applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<RequestResult<IEnumerable<ApplicationResponse>>> ExecuteAsync(Guid request)
    {
        var result = await _applicationRepository.GetAllAsync(1, 1000);
        if (result.Status != EResultStatus.Success)
            return RequestResult<IEnumerable<ApplicationResponse>>.WithError(result.Message);

        var applications = result.Data!
            .Where(a => a.JobId.Value == request)
            .Select(MapToResponse)
            .OrderByDescending(a => a.CreatedAt);

        return RequestResult<IEnumerable<ApplicationResponse>>.Success(applications);
    }

    private static ApplicationResponse MapToResponse(Application app) => new()
    {
        Id = app.Id.Value,
        CandidateId = app.CandidateId.Value,
        JobId = app.JobId.Value,
        Status = app.Status.ToString(),
        AppliedAt = app.AppliedAt,
        ViewedAt = app.ViewedAt,
        SelectedAt = app.SelectedAt,
        RejectedAt = app.RejectedAt,
        Notes = app.Notes,
        CreatedAt = app.CreatedAt
    };
}
