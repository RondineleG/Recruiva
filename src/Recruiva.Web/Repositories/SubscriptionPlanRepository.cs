using Microsoft.EntityFrameworkCore;

using Recruiva.Core.Entities;
using Recruiva.Core.Enums;
using Recruiva.Core.Interfaces.Repositories.Base;
using Recruiva.Core.Requests;
using Recruiva.Core.ValueObjects;

namespace Recruiva.Web.Repositories;

public class SubscriptionPlanRepository : IBaseRepository<SubscriptionPlan>
{
    private readonly ApplicationDbContext _context;

    public SubscriptionPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResult<SubscriptionPlan>> CreateAsync(SubscriptionPlan entity)
    {
        _context.SubscriptionPlans.Add(entity);
        await _context.SaveChangesAsync();
        return RequestResult<SubscriptionPlan>.Success(entity);
    }

    public async Task<RequestResult<SubscriptionPlan>> DeleteAsync(Id id)
    {
        var plan = await _context.SubscriptionPlans.FindAsync(id.Value);
        if (plan == null)
            return RequestResult<SubscriptionPlan>.EntityNotFound("SubscriptionPlan", id.Value, "Plano não encontrado.");

        plan.IsDeleted = true;
        plan.DeletedAt = DateTime.UtcNow;

        _context.SubscriptionPlans.Update(plan);
        await _context.SaveChangesAsync();
        return RequestResult<SubscriptionPlan>.Success(plan);
    }

    public async Task<RequestResult<IEnumerable<SubscriptionPlan>>> GetAllAsync(int page, int size)
    {
        var plans = await _context.SubscriptionPlans
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.Price)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return RequestResult<IEnumerable<SubscriptionPlan>>.Success(plans);
    }

    public async Task<RequestResult<SubscriptionPlan>> GetByIdAsync(Id id)
    {
        var plan = await _context.SubscriptionPlans
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (plan == null)
            return RequestResult<SubscriptionPlan>.EntityNotFound("SubscriptionPlan", id.Value, "Plano não encontrado.");

        return RequestResult<SubscriptionPlan>.Success(plan);
    }

    public async Task<RequestResult<SubscriptionPlan>> UpdateAsync(SubscriptionPlan entity)
    {
        _context.SubscriptionPlans.Update(entity);
        await _context.SaveChangesAsync();
        return RequestResult<SubscriptionPlan>.Success(entity);
    }
}
