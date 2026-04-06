using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Candidates;

public sealed class UpdateCandidateUseCase : IUseCase<UpdateCandidateRequest, CandidateResponse>
{
    private readonly IBaseRepository<Candidate> _candidateRepository;

    public UpdateCandidateUseCase(IBaseRepository<Candidate> candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<RequestResult<CandidateResponse>> ExecuteAsync(UpdateCandidateRequest request)
    {
        var getResult = await _candidateRepository.GetByIdAsync(Id.Create(request.Id));

        if (getResult.Status != EResultStatus.Success || getResult.Data == null)
            return RequestResult<CandidateResponse>.EntityNotFound("Candidate", request.Id, "Candidato não encontrado.");

        var candidate = getResult.Data;

        candidate.Name = request.Name;
        candidate.Phone = request.Phone;
        candidate.DateOfBirth = request.DateOfBirth;
        candidate.LinkedIn = request.LinkedIn;

        if (request.AddressId.HasValue)
        {
            candidate.AddressId = Id.Create(request.AddressId.Value);
        }

        var result = await _candidateRepository.UpdateAsync(candidate);

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
