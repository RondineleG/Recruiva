using Recruiva.Core.DTOs.Request;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Core.UseCases.Subscriptions;

public sealed class CancelSubscriptionUseCase : IUseCase<CancelSubscriptionRequest, RequestResult>
{
    private readonly IBaseRepository<Subscription> _subscriptionRepository;
    private readonly IBaseRepository<Advertiser> _advertiserRepository;

    public CancelSubscriptionUseCase(
        IBaseRepository<Subscription> subscriptionRepository,
        IBaseRepository<Advertiser> advertiserRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _advertiserRepository = advertiserRepository;
    }

    public async Task<RequestResult<RequestResult>> ExecuteAsync(CancelSubscriptionRequest request)
    {
        // Buscar assinatura
        var subscriptionResult = await _subscriptionRepository.GetByIdAsync(Id.Create(request.SubscriptionId));
        if (subscriptionResult.Status != EResultStatus.Success || subscriptionResult.Data == null)
            return RequestResult<RequestResult>.EntityNotFound("Subscription", request.SubscriptionId, "Assinatura não encontrada.");

        var subscription = subscriptionResult.Data;

        // Verificar se está ativa
        if (subscription.Status != ESubscriptionStatus.Active)
            return RequestResult<RequestResult>.WithError("A assinatura não está ativa e não pode ser cancelada.");

        // Cancelar assinatura
        subscription.Status = ESubscriptionStatus.Cancelled;
        subscription.EndDate = DateTime.UtcNow;
        subscription.CancellationReason = request.Reason;
        await _subscriptionRepository.UpdateAsync(subscription);

        // Voltar para plano Free
        var advertiserResult = await _advertiserRepository.GetByIdAsync(subscription.AdvertiserId);
        if (advertiserResult.Status == EResultStatus.Success && advertiserResult.Data != null)
        {
            var advertiser = advertiserResult.Data;
            advertiser.ActivePlan = "Free";
            await _advertiserRepository.UpdateAsync(advertiser);
        }

        return RequestResult<RequestResult>.Success(RequestResult.Success());
    }
}
