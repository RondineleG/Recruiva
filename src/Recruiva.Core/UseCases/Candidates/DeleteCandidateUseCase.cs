using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Candidates;

public sealed class DeleteCandidateUseCase : IUseCase<Guid, bool>
{
    private readonly IBaseRepository<Candidate> _candidateRepository;

    public DeleteCandidateUseCase(IBaseRepository<Candidate> candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(Guid request)
    {
        var result = await _candidateRepository.DeleteAsync(Id.Create(request));
        if (result.Status != EResultStatus.Success)
            return RequestResult<bool>.WithError(result.Message);

        return RequestResult<bool>.Success(true);
    }
}
