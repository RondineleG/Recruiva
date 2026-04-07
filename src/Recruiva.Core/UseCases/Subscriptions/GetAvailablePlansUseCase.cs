using Recruiva.Core.DTOs.Response;
using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Interfaces.UseCases;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Recruiva.Core.UseCases.Subscriptions;

public sealed class GetAvailablePlansUseCase : IUseCase<Guid, AvailablePlansResponse>
{
    private readonly IBaseRepository<SubscriptionPlan> _planRepository;
    private readonly IBaseRepository<Subscription> _subscriptionRepository;

    public GetAvailablePlansUseCase(
        IBaseRepository<SubscriptionPlan> planRepository,
        IBaseRepository<Subscription> subscriptionRepository)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<RequestResult<AvailablePlansResponse>> ExecuteAsync(Guid advertiserId)
    {
        // Listar todos os planos ativos
        var plansResult = await _planRepository.GetAllAsync(1, 100);
        if (plansResult.Status != EResultStatus.Success)
            return RequestResult<AvailablePlansResponse>.WithError(plansResult.Message);

        var plans = plansResult.Data!
            .Where(p => p.IsActive)
            .Select(MapToPlanResponse)
            .ToList();

        // Buscar assinatura atual do anunciante
        SubscriptionResponse? currentSubscription = null;
        var subscriptionsResult = await _subscriptionRepository.GetAllAsync(1, 100);
        if (subscriptionsResult.Status == EResultStatus.Success)
        {
            var activeSubscription = subscriptionsResult.Data!
                .FirstOrDefault(s => s.AdvertiserId == Id.Create(advertiserId)
                    && s.Status == ESubscriptionStatus.Active);

            if (activeSubscription != null)
            {
                currentSubscription = new SubscriptionResponse
                {
                    Id = activeSubscription.Id.Value,
                    PlanName = activeSubscription.Plan?.Name ?? "Desconhecido",
                    Status = activeSubscription.Status.ToString(),
                    StartDate = activeSubscription.StartDate,
                    EndDate = activeSubscription.EndDate,
                    Price = activeSubscription.Plan?.Price ?? 0
                };
            }
        }

        var response = new AvailablePlansResponse
        {
            Plans = plans,
            CurrentSubscription = currentSubscription
        };

        return RequestResult<AvailablePlansResponse>.Success(response);
    }

    private static PlanResponse MapToPlanResponse(SubscriptionPlan p) => new()
    {
        Id = p.Id.Value,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        MaxJobs = p.MaxJobs,
        HasBoost = p.HasBoost,
        HasHighlight = p.HasHighlight,
        HasAnalytics = p.HasAnalytics,
        MaxResumes = p.MaxResumes
    };
}
