using Recruiva.Core.DTOs.Request;
using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Advertisers;

public class CreateAdvertiserUseCase : IUseCase<CreateAdvertiserRequest, AdvertiserResponse>
{
    private readonly IBaseRepository<Advertiser> _advertiserRepository;

    public CreateAdvertiserUseCase(IBaseRepository<Advertiser> advertiserRepository)
    {
        _advertiserRepository = advertiserRepository;
    }

    public async Task<RequestResult<AdvertiserResponse>> ExecuteAsync(CreateAdvertiserRequest request)
    {
        var advertiser = new Advertiser
        {
            Id = Id.Create(Guid.NewGuid()),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            TaxId = request.TaxId,
            PersonType = request.PersonType,
            CompanyDescription = request.CompanyDescription,
            Website = request.Website,
            LogoUrl = request.LogoUrl,
            Status = EAdvertiserStatus.Incomplete,
            IsEmailVerified = false,
            IsPhoneVerified = false,
            ActivePlan = "Free",
            TenantId = "default"
        };

        var result = await _advertiserRepository.CreateAsync(advertiser);
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
