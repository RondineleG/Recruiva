using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Candidates;

public class CreateCandidateUseCase : IUseCase<CreateCandidateRequest, CandidateResponse>
{
    private readonly IBaseRepository<Candidate> _candidateRepository;

    public CreateCandidateUseCase(IBaseRepository<Candidate> candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<RequestResult<CandidateResponse>> ExecuteAsync(CreateCandidateRequest request)
    {
        var candidate = new Candidate
        {
            Id = Id.Create(Guid.NewGuid()),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            LinkedIn = request.LinkedIn,
            Status = EAccountStatus.Incomplete,
            IsEmailVerified = false,
            IsPhoneVerified = false,
            TenantId = "default"
        };

        var result = await _candidateRepository.CreateAsync(candidate);
        if (result.Status != EResultStatus.Success)
            return RequestResult<CandidateResponse>.WithError(result.Message);

        return RequestResult<CandidateResponse>.Success(MapToResponse(result.Data!));
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
        CreatedAt = c.CreatedAt
    };
}
