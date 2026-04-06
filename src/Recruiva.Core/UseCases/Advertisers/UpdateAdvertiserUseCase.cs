using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Advertisers;

public sealed class UpdateAdvertiserUseCase : IUseCase<UpdateAdvertiserRequest, AdvertiserResponse>
{
    private readonly IBaseRepository<Advertiser> _advertiserRepository;

    public UpdateAdvertiserUseCase(IBaseRepository<Advertiser> advertiserRepository)
    {
        _advertiserRepository = advertiserRepository;
    }

    public async Task<RequestResult<AdvertiserResponse>> ExecuteAsync(UpdateAdvertiserRequest request)
    {
        var getResult = await _advertiserRepository.GetByIdAsync(Id.Create(request.Id));

        if (getResult.Status != EResultStatus.Success || getResult.Data == null)
            return RequestResult<AdvertiserResponse>.EntityNotFound("Advertiser", request.Id, "Anunciante não encontrado.");

        var advertiser = getResult.Data;

        advertiser.Name = request.Name;
        advertiser.Phone = request.Phone;
        advertiser.CompanyDescription = request.CompanyDescription;
        advertiser.Website = request.Website;
        advertiser.LogoUrl = request.LogoUrl;
        advertiser.PersonType = request.PersonType;

        if (request.AddressId.HasValue)
        {
            advertiser.AddressId = Id.Create(request.AddressId.Value);
        }

        var result = await _advertiserRepository.UpdateAsync(advertiser);

        if (result.Status != EResultStatus.Success)
            return RequestResult<AdvertiserResponse>.WithError(result.Message);

        return RequestResult<AdvertiserResponse>.Success(MapToResponse(result.Data!));
    }

    private static AdvertiserResponse MapToResponse(Advertiser a) => new()
    {
        Id = a.Id.Value,
        Name = a.Name,
        Email = a.Email,
        Phone = a.Phone,
        TaxId = a.TaxId,
        PersonType = a.PersonType.ToString(),
        CompanyDescription = a.CompanyDescription,
        Website = a.Website,
        LogoUrl = a.LogoUrl,
        Status = a.Status.ToString(),
        IsEmailVerified = a.IsEmailVerified,
        ActivePlan = a.ActivePlan,
        CreatedAt = a.CreatedAt
    };
}
