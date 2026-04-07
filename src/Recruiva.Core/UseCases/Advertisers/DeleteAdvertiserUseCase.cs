using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Advertisers;

public sealed class DeleteAdvertiserUseCase : IUseCase<Guid, bool>
{
    private readonly IBaseRepository<Advertiser> _advertiserRepository;

    public DeleteAdvertiserUseCase(IBaseRepository<Advertiser> advertiserRepository)
    {
        _advertiserRepository = advertiserRepository;
    }

    public async Task<RequestResult<bool>> ExecuteAsync(Guid request)
    {
        var result = await _advertiserRepository.DeleteAsync(Id.Create(request));
        if (result.Status != EResultStatus.Success)
            return RequestResult<bool>.WithError(result.Message);

        return RequestResult<bool>.Success(true);
    }
}
