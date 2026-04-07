using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Candidates;

public sealed class GetCandidateByIdUseCase : IUseCase<Guid, CandidateResponse>
{
    private readonly IBaseRepository<Candidate> _candidateRepository;

    public GetCandidateByIdUseCase(IBaseRepository<Candidate> candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<RequestResult<CandidateResponse>> ExecuteAsync(Guid request)
    {
        var result = await _candidateRepository.GetByIdAsync(Id.Create(request));
        if (result.Status != EResultStatus.Success)
            return RequestResult<CandidateResponse>.WithError(result.Message);

        var candidate = result.Data!;
        var response = MapToResponse(candidate);

        return RequestResult<CandidateResponse>.Success(response);
    }

    private static CandidateResponse MapToResponse(Candidate c) => new()
    {
        Id = c.Id.Value,
        Name = c.Name,
        Email = c.Email,
        Phone = c.Phone,
        DateOfBirth = c.DateOfBirth,
        LinkedIn = c.LinkedIn,
        Status = c.Status.ToString(),
        IsEmailVerified = c.IsEmailVerified,
        IsPhoneVerified = c.IsPhoneVerified,
        City = c.Address?.City,
        State = c.Address?.State,
        ResumesCount = c.Resumes?.Count ?? 0,
        ApplicationsCount = c.Applications?.Count ?? 0,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };
}
