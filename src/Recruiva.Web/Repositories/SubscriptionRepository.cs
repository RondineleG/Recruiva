using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class SubscriptionRepository : IBaseRepository<Subscription>
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<Subscription>> CreateAsync(Subscription entity)
    {
        _context.Subscriptions.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Subscription>.Success(entity);
    }

    public async Task<RequestResult<Subscription>> DeleteAsync(Id id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id.Value);
        if (subscription == null)
            return RequestResult<Subscription>.EntityNotFound("Subscription", id.Value, "Assinatura não encontrada.");

        subscription.IsDeleted = true;
        subscription.DeletedAt = DateTime.UtcNow;

        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
        return RequestResult<Subscription>.Success(subscription);
    }

    public async Task<RequestResult<IEnumerable<Subscription>>> GetAllAsync(int page, int size)
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Plan)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.StartDate)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<Subscription>>.Success(subscriptions);
    }

    public async Task<RequestResult<Subscription>> GetByIdAsync(Id id)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (subscription == null)
            return RequestResult<Subscription>.EntityNotFound("Subscription", id.Value, "Assinatura não encontrada.");

        return RequestResult<Subscription>.Success(subscription);
    }

    public async Task<RequestResult<Subscription>> UpdateAsync(Subscription entity)
    {
        _context.Subscriptions.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<Subscription>.Success(entity);
    }

    /// <summary>
    /// Busca todas as assinaturas de um anunciante.
    /// </summary>
    public async Task<RequestResult<IEnumerable<Subscription>>> GetByAdvertiserIdAsync(Id advertiserId)
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Plan)
            .Where(s => s.AdvertiserId == advertiserId && !s.IsDeleted)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();

        return RequestResult<IEnumerable<Subscription>>.Success(subscriptions);
    }

    /// <summary>
    /// Busca a assinatura ativa de um anunciante.
    /// </summary>
    public async Task<RequestResult<Subscription?>> GetActiveByAdvertiserIdAsync(Id advertiserId)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.AdvertiserId == advertiserId
                && s.Status == ESubscriptionStatus.Active
                && !s.IsDeleted);

        return RequestResult<Subscription?>.Success(subscription);
    }
}
