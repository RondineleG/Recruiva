using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Applications;

public class CreateApplicationUseCase : IUseCase<CreateApplicationRequest, ApplicationResponse>
{
    private readonly IBaseRepository<Application> _applicationRepository;
    private readonly IBaseRepository<Job> _jobRepository;

    public CreateApplicationUseCase(IBaseRepository<Application> applicationRepository, IBaseRepository<Job> jobRepository)
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
    }

    public async Task<RequestResult<ApplicationResponse>> ExecuteAsync(CreateApplicationRequest request)
    {
        // Verificar se vaga existe e está ativa
        var jobResult = await _jobRepository.GetByIdAsync(Id.Create(request.JobId));
        if (jobResult.Status != EResultStatus.Success)
            return RequestResult<ApplicationResponse>.WithError("Vaga não encontrada.");

        var job = jobResult.Data!;
        if (job.Status != EJobStatus.Active)
            return RequestResult<ApplicationResponse>.WithError("Vaga não está mais ativa.");

        // Criar candidatura
        var application = new Application
        {
            Id = Id.Create(Guid.NewGuid()),
            CandidateId = Id.Create(request.CandidateId),
            JobId = Id.Create(request.JobId),
            Status = EApplicationStatus.Sent,
            AppliedAt = DateTime.UtcNow,
            Notes = request.Notes,
            TenantId = "default"
        };

        var result = await _applicationRepository.CreateAsync(application);
        if (result.Status != EResultStatus.Success)
            return RequestResult<ApplicationResponse>.WithError(result.Message);

        // Incrementar contador de aplicações da vaga
        if (job.Counters != null)
        {
            job.Counters.Applications++;
            await _jobRepository.UpdateAsync(job);
        }

        return RequestResult<ApplicationResponse>.Success(MapToResponse(result.Data!));
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
